using System.Collections.Generic;

namespace CommonInteraction
{
    public class CommandInfo
    {
        public readonly User User;
        public readonly CommandType CommandType;
        public readonly bool IsComChat;
        public readonly ulong ComChatId;
        public readonly IEnumerable<string> MentPlayers;
        public readonly IEnumerable<string> Content;

        public CommandInfo(User user,
            CommandType command,
            bool isComChat,
            ulong comChatId = 0,
            IEnumerable<string> mentPlayers = null, 
            IEnumerable<string> content = null)
        {
            CommandType = command;
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            MentPlayers = mentPlayers;
            Content = content;
        }
    }
}