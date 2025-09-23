using Microsoft.AspNetCore.SignalR;
using Organetto.UseCases.Shared.SignalR.Models;

namespace Organetto.UseCases.Shared.SignalR.Services
{
    public class UserHub<TClient> : Hub<TClient>
        where TClient : class
    {
        /// <summary>
        /// Called by the client immediately after connection to join its personal group.
        /// </summary>
        public Task Subscribe(long userId)
        {
            var groupName = SignalRGroupNames.UserGroup(userId);
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Optional: client can leave the group when navigating away.
        /// </summary>
        public Task Unsubscribe(long userId)
        {
            var groupName = SignalRGroupNames.UserGroup(userId);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        /// userId приходит в query ?userId=42
        public override async Task OnConnectedAsync()
        {
            var userIdParam = Context.GetHttpContext()!.Request.Query["userId"];
            if (!long.TryParse(userIdParam, out var userId) || userId <= 0)
            {
                Context.Abort(); // защищаем хаб
                return;
            }

            await Subscribe(userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var userIdParam = Context.GetHttpContext()!.Request.Query["userId"];
            if (long.TryParse(userIdParam, out var userId) && userId > 0)
                await Unsubscribe(userId);

            await base.OnDisconnectedAsync(ex);
        }
    }
}
