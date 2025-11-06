using System;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace InventoryManager.Services
{
    public class I18nService : II18nService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;

        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set => _currentCulture = value;
        }

        public I18nService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _resourceManager = new ResourceManager("InventoryManager.Resources.SharedResource", typeof(I18nService).Assembly);

            // Initialize with browser language or default to English
            var defaultCulture = new CultureInfo("en-US");
            _currentCulture = defaultCulture;
        }

        public string GetLocalizedText(string key)
        {
            try
            {
                return _resourceManager.GetString(key, _currentCulture) ?? key;
            }
            catch
            {
                return key;
            }
        }

        public string GetLocalizedText(string key, params object[] args)
        {
            try
            {
                var format = _resourceManager.GetString(key, _currentCulture);
                if (format == null)
                    return key;

                return string.Format(format, args);
            }
            catch
            {
                return key;
            }
        }

        public async Task SetLanguageAsync(string languageCode)
        {
            try
            {
                _currentCulture = new CultureInfo(languageCode);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "language", languageCode);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error setting language: {ex.Message}");
            }
        }

        public string GetCurrentLanguage()
        {
            return _currentCulture.Name;
        }
    }
}
