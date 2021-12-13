using System.Collections.Generic;

namespace CommonInteraction
{
    public class CommandInfo
    {
        public readonly User User;
        public readonly bool IsCommonChat;
        public readonly CommandType CommandType;
        public readonly IEnumerable<string> MentionedPlayers;
        public readonly IEnumerable<string> Content;

        public CommandInfo(User user,  bool isCommonChat, CommandType command,
            IEnumerable<string> mentionedPlayers = null, 
            IEnumerable<string> content = null)
        {
            CommandType = command;
            User = user;
            IsCommonChat = isCommonChat;
            MentionedPlayers = mentionedPlayers;
            Content = content;
        }
    }
}