using Microsoft.AspNetCore.SignalR;
using Organetto.UseCases.Boards.Services;

namespace Organetto.UseCases.Boards.Hubs
{
    public class BoardHub : Hub<IBoardClient>
    {
        /// <summary>
        /// Called by the client immediately after connection to join its personal group.
        /// </summary>
        public Task Subscribe(long userId)
        {
            var groupName = GetGroupName(userId);
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Optional: client can leave the group when navigating away.
        /// </summary>
        public Task Unsubscribe(long userId)
        {
            var groupName = GetGroupName(userId);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        private static string GetGroupName(long userId) =>
            $"boards:{userId}";

        ///// userId приходит в query ?userId=42
        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.GetHttpContext()!.Request.Query["userId"];
        //    if (!long.TryParse(userId, out var id) || id <= 0)
        //    {
        //        Context.Abort(); // защищаем хаб
        //        return;
        //    }

        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"boards:{id}");
        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception? ex)
        //{
        //    var userId = Context.GetHttpContext()!.Request.Query["userId"];
        //    if (long.TryParse(userId, out var id) && id > 0)
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"boards:{id}");

        //    await base.OnDisconnectedAsync(ex);
        //}
    }
}
