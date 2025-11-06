using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationDbContext dbContext, ILogger<AuthController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string provider, [FromQuery] string returnUrl = "/")
        {
            _logger.LogInformation("Login attempt with provider: {Provider}, returnUrl: {ReturnUrl}", provider, returnUrl);
            
            if (string.IsNullOrEmpty(provider))
            {
                _logger.LogWarning("Login attempt with null or empty provider");
                return BadRequest();
            }

            var properties = new AuthenticationProperties { RedirectUri = returnUrl };
            _logger.LogInformation("Challenging authentication for provider: {Provider}", provider);
            return Challenge(properties, provider);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Logout attempt");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}