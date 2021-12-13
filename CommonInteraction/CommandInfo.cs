using System.Collections.Generic;

namespace CommonInteraction
{
    public class CommandInfo
    {
        public CommandType CommandType { get; }
        public IReadOnlyCollection<string> MentionedPlayers { get; }
        public IReadOnlyList<string> Content { get; }

        public CommandInfo(CommandType command, IReadOnlyCollection<string> mentionedPlayers, IReadOnlyList<string> content)
        {
            CommandType = command;
            MentionedPlayers = mentionedPlayers;
            Content = content;
        }
    }
}