using Microsoft.EntityFrameworkCore;
using InventoryManager.Data;
using InventoryManager.Models;

namespace InventoryManager.Services
{
    public interface IAdminAccessService
    {
        Task<AdminAccess> GenerateAdminLinkAsync(string userId, string userEmail);
        Task<AdminAccess?> ValidateLinkAsync(string link);
        Task<bool> UseLinkAsync(string link, string userId, string userEmail);
        Task<List<AdminAccess>> GetAllLinksAsync();
        Task<bool> DeleteLinkAsync(Guid id);
    }

    public class AdminAccessService : IAdminAccessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminAccessService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AdminAccess> GenerateAdminLinkAsync(string userId, string userEmail)
        {
            var link = Guid.NewGuid().ToString();
            var baseUrl = GetBaseUrl();
            var fullLink = $"{baseUrl}/admin-access/{link}";

            var adminAccess = new AdminAccess
            {
                GeneratedLink = link,
                UserId = userId,
                UserEmail = userEmail,
                GeneratedAt = DateTime.UtcNow
            };

            _context.AdminAccesses.Add(adminAccess);
            await _context.SaveChangesAsync();

            return adminAccess;
        }

        public async Task<AdminAccess?> ValidateLinkAsync(string link)
        {
            return await _context.AdminAccesses
                .FirstOrDefaultAsync(a => a.GeneratedLink == link && !a.IsUsed);
        }

        public async Task<bool> UseLinkAsync(string link, string userId, string userEmail)
        {
            var adminAccess = await ValidateLinkAsync(link);

            if (adminAccess == null)
            {
                return false;
            }

            adminAccess.IsUsed = true;
            adminAccess.UsedAt = DateTime.UtcNow;
            adminAccess.UsedByUserId = userId;
            adminAccess.UsedByEmail = userEmail;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AdminAccess>> GetAllLinksAsync()
        {
            return await _context.AdminAccesses
                .OrderByDescending(a => a.GeneratedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteLinkAsync(Guid id)
        {
            var adminAccess = await _context.AdminAccesses.FindAsync(id);
            if (adminAccess == null)
            {
                return false;
            }

            _context.AdminAccesses.Remove(adminAccess);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return "http://localhost:5000";
            }

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return baseUrl;
        }
    }
}
