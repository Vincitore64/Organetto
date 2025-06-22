namespace Organetto.UseCases.Shared.SignalR.Models
{
    public static class SignalRGroupNames
    {
        private const string BoardsPrefix = "boards:";

        /// <summary>
        /// Returns the SignalR group name for a given user’s boards channel.
        /// </summary>
        public static string BoardsGroup(long userId)
            => $"{BoardsPrefix}{userId}";
    }
}
