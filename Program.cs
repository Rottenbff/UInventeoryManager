using InventoryManager.Components;
using InventoryManager.Data;
using InventoryManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using InventoryManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Localization
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("es-ES")
    };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add DbContext configuration with detailed logging
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

// Register services
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<ICustomIdGeneratorService, CustomIdGeneratorService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IInventoryAccessService, InventoryAccessService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<II18nService, I18nService>();
builder.Services.AddScoped<IDragDropInteropService, DragDropInteropService>();
builder.Services.AddScoped<IAdminAccessService, AdminAccessService>();
builder.Services.AddScoped<ApplicationDbContext>();

// Add Authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/Account/AccessDenied";

        // Configure cookie for OAuth correlation
        options.Cookie.Name = "InventoryManagerAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Set to Required in production with HTTPS
        options.Cookie.IsEssential = true;

        // Increase timeout for OAuth flow
        options.ExpireTimeSpan = TimeSpan.FromHours(1);

        // Sliding expiration for better UX
        options.SlidingExpiration = true;

        // Check for cookies on every request
        options.Events.OnValidatePrincipal = context =>
        {
            // Check if user is blocked
            if (context.Principal?.Identity?.IsAuthenticated == true)
            {
                var email = context.Principal.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(email))
                {
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                    var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
                    if (user?.IsBlocked == true)
                    {
                        context.RejectPrincipal();
                        context.HttpContext.Response.Redirect("/user-blocked");
                    }
                }
            }
            return Task.CompletedTask;
        };

        // Handle OAuth correlation failures
        options.Events.OnRedirectToLogin = context =>
        {
            // Don't redirect if it's an API request
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            // Add error parameter if correlation failed
            if (context.Response.StatusCode == 302 &&
                context.RedirectUri?.Contains("error=correlation") == true)
            {
                context.Response.Redirect("/login?error=" + Uri.EscapeDataString("Authentication failed. Please try again."));
                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;

        // Configure OAuth correlation cookie
        options.CorrelationCookie.Name = ".InventoryManager.Correlation.Google";
        options.CorrelationCookie.HttpOnly = true;
        options.CorrelationCookie.SameSite = SameSiteMode.Lax;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None; // Set to Required in production

        options.Events.OnCreatingTicket = async context =>
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            var email = context.Principal!.FindFirstValue(ClaimTypes.Email);
            if (email is null) return;

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Name = context.Principal!.FindFirstValue(ClaimTypes.Name),
                    Provider = "Google",
                    ProviderId = context.Principal!.FindFirstValue(ClaimTypes.NameIdentifier),
                    IsAdmin = false,
                    IsBlocked = false
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            // Check if user is blocked
            if (user.IsBlocked)
            {
                context.Fail("User is blocked");
                return;
            }

            // Update the principal with user information
            var claimsIdentity = (ClaimsIdentity)context.Principal!.Identity!;
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new Claim("Name", user.Name ?? ""));
            claimsIdentity.AddClaim(new Claim("Provider", user.Provider ?? ""));
            
            if (user.IsAdmin)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }
        };
        
        options.Events.OnRemoteFailure = context =>
        {
            // Log the failure reason
            Console.WriteLine($"OAuth Remote Failure: {context.Failure?.Message}");

            // Check if the failure is due to a blocked user
            // Only access form if the request has form content
            if (context.Request.HasFormContentType &&
                context.Request.Form.ContainsKey("error_description"))
            {
                var errorDescription = context.Request.Form["error_description"].FirstOrDefault();
                if (!string.IsNullOrEmpty(errorDescription) && errorDescription.Contains("User is blocked"))
                {
                    context.HandleResponse();
                    context.Response.Redirect("/user-blocked");
                    return Task.CompletedTask;
                }
            }

            // For other failures, redirect to login page with error
            context.HandleResponse();
            context.Response.Redirect("/login?error=" + Uri.EscapeDataString(context.Failure?.Message ?? "Authentication failed"));
            return Task.CompletedTask;
        };
        
        // Add logging for Google authentication events
        options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            Console.WriteLine($"Redirecting to Google authorization endpoint: {context.RedirectUri}");
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;

        // Configure OAuth correlation cookie
        options.CorrelationCookie.Name = ".InventoryManager.Correlation.Facebook";
        options.CorrelationCookie.HttpOnly = true;
        options.CorrelationCookie.SameSite = SameSiteMode.Lax;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None; // Set to Required in production

        options.Events.OnCreatingTicket = async context =>
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            var email = context.Principal!.FindFirstValue(ClaimTypes.Email);
            if (email is null) return;

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Name = context.Principal!.FindFirstValue(ClaimTypes.Name),
                    Provider = "Facebook",
                    ProviderId = context.Principal!.FindFirstValue(ClaimTypes.NameIdentifier),
                    IsAdmin = false,
                    IsBlocked = false
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
            
            // Check if user is blocked
            if (user.IsBlocked)
            {
                context.Fail("User is blocked");
                return;
            }

            // Update the principal with user information
            var claimsIdentity = (ClaimsIdentity)context.Principal!.Identity!;
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new Claim("Name", user.Name ?? ""));
            claimsIdentity.AddClaim(new Claim("Provider", user.Provider ?? ""));
            
            if (user.IsAdmin)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }
        };
        
        options.Events.OnRemoteFailure = context =>
        {
            // Log the failure reason
            Console.WriteLine($"OAuth Remote Failure: {context.Failure?.Message}");

            // Check if the failure is due to a blocked user
            // Only access form if the request has form content
            if (context.Request.HasFormContentType &&
                context.Request.Form.ContainsKey("error_description"))
            {
                var errorDescription = context.Request.Form["error_description"].FirstOrDefault();
                if (!string.IsNullOrEmpty(errorDescription) && errorDescription.Contains("User is blocked"))
                {
                    context.HandleResponse();
                    context.Response.Redirect("/user-blocked");
                    return Task.CompletedTask;
                }
            }

            // For other failures, redirect to login page with error
            context.HandleResponse();
            context.Response.Redirect("/login?error=" + Uri.EscapeDataString(context.Failure?.Message ?? "Authentication failed"));
            return Task.CompletedTask;
        };
        
        // Add logging for Facebook authentication events
        options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            Console.WriteLine($"Redirecting to Facebook authorization endpoint: {context.RedirectUri}");
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRequestLocalization();

app.UseRouting();

// Add middleware to check if user is blocked on each request
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
        if (!string.IsNullOrEmpty(email))
        {
            var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user != null && user.IsBlocked)
            {
                // Sign out the user and redirect to blocked page
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/user-blocked");
                return;
            }
        }
    }
    
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Map SignalR hubs
app.MapHub<InventoryManager.Hubs.CommentHub>("/hubs/comments");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();