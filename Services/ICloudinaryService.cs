using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile imageFile);
        Task<string?> UploadImageAsync(IBrowserFile imageFile);
        Task<bool> DeleteImageAsync(string publicId);
    }
}