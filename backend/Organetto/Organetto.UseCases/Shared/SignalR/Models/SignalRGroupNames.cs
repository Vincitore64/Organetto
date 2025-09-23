namespace Organetto.UseCases.Shared.SignalR.Models
{
    public static class SignalRGroupNames
    {
        private const string boardsPrefix = "boards:";
        private const string boardPrefix = "board:";
        private const string userPrefix = "user:";

        /// <summary>
        /// Returns the SignalR group name for a given user’s boards channel.
        /// </summary>
        public static string BoardsGroup(long userId)
            => $"{boardsPrefix}{userId}";

        public static string BoardGroup(long boardId)
            => $"{boardPrefix}{boardId}";

        public static string UserGroup(long userId)
            => $"{userPrefix}{userId}";
    }
}
