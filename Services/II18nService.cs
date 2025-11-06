using System.Globalization;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface II18nService
    {
        string GetLocalizedText(string key);
        string GetLocalizedText(string key, params object[] args);
        CultureInfo CurrentCulture { get; set; }
        Task SetLanguageAsync(string languageCode);
        string GetCurrentLanguage();
    }
}
