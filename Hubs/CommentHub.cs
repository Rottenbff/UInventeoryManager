using Microsoft.AspNetCore.SignalR;

namespace InventoryManager.Hubs
{
    public class CommentHub : Hub
    {
        /// <summary>
        /// Joins a comment group for a specific inventory
        /// </summary>
        /// <param name="inventoryId">The ID of the inventory to join</param>
        public async Task JoinInventoryGroup(string inventoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Inventory_{inventoryId}");
            Console.WriteLine($"Client {Context.ConnectionId} joined inventory group: {inventoryId}");
        }

        /// <summary>
        /// Leaves a comment group for a specific inventory
        /// </summary>
        /// <param name="inventoryId">The ID of the inventory to leave</param>
        public async Task LeaveInventoryGroup(string inventoryId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Inventory_{inventoryId}");
            Console.WriteLine($"Client {Context.ConnectionId} left inventory group: {inventoryId}");
        }
    }
}
