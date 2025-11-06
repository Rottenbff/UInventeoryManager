using Microsoft.JSInterop;

namespace InventoryManager.Services
{
    public interface IDragDropInteropService
    {
        Task InitializeSortable<T>(string elementId, DotNetObjectReference<T> dotNetObject, string callbackMethod) where T : class;
        Task DestroySortable(string elementId);
    }

    public class DragDropInteropService : IDragDropInteropService
    {
        private readonly IJSRuntime _jsRuntime;

        public DragDropInteropService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeSortable<T>(string elementId, DotNetObjectReference<T> dotNetObject, string callbackMethod) where T : class
        {
            await _jsRuntime.InvokeVoidAsync("initializeSortable", elementId, dotNetObject, callbackMethod);
        }

        public async Task DestroySortable(string elementId)
        {
            await _jsRuntime.InvokeVoidAsync("destroySortable", elementId);
        }
    }
}
