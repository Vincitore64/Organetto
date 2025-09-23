using Microsoft.AspNetCore.SignalR;
using Organetto.UseCases.Shared.SignalR.Models;

namespace Organetto.UseCases.Shared.SignalR.Services
{
    public abstract class BaseBoardHub<TClient> : Hub<TClient>
    where TClient : class
    {
        // this method is called by the client immediately after connection
        /// <summary>
        /// Joins the client to the group for this board.
        /// </summary>
        public Task SubscribeBoard(long boardId)
            => Groups.AddToGroupAsync(
                   Context.ConnectionId,
                   SignalRGroupNames.BoardGroup(boardId));

        /// <summary>
        /// Leaves the client from the group.
        /// </summary>
        public Task UnsubscribeBoard(long boardId)
            => Groups.RemoveFromGroupAsync(
                   Context.ConnectionId,
                   SignalRGroupNames.BoardGroup(boardId));
    }
}
