using Microsoft.AspNetCore.SignalR;
using Organetto.UseCases.Shared.SignalR.Models;

namespace Organetto.UseCases.Shared.SignalR.Services.Extensions
{
    internal static class UserHubExtensions
    {
        public static T UserGroup<T>(this IHubClients<T> hubClients, long userId)
        {
            return hubClients.Group(SignalRGroupNames.UserGroup(userId));
        }
    }
}
