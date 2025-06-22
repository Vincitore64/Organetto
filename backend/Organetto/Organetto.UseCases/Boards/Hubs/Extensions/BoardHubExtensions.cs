using Microsoft.AspNetCore.SignalR;
using Organetto.UseCases.Shared.SignalR.Models;

namespace Organetto.UseCases.Boards.Hubs.Extensions
{
    public static class BoardHubExtensions
    {
        public static T BoardGroup<T>(this IHubClients<T> hubClients, long userId)
        {
            return hubClients.Group(SignalRGroupNames.BoardsGroup(userId));
        }
    }
}
