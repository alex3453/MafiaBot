using System.Collections.Generic;

namespace CommonInteraction
{
    public class CommandInfo
    {
        public readonly User User;
        public readonly bool IsCommonChannel;
        public readonly CommandType CommandType;
        public readonly IEnumerable<string> MentionedPlayers;
        public readonly IEnumerable<string> Content;

        public CommandInfo(User user,  bool isCommonChannel, CommandType command,
            IEnumerable<string> mentionedPlayers = null, 
            IEnumerable<string> content = null)
        {
            CommandType = command;
            User = user;
            IsCommonChannel = isCommonChannel;
            MentionedPlayers = mentionedPlayers;
            Content = content;
        }
    }
}