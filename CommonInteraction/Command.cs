using System.Collections.Generic;

namespace CommonInteraction
{
    public class Command
    {
        public CommandType CommandType { get; }
        public IReadOnlyCollection<string> MentionedPlayers { get; }

        public Command(CommandType command, IReadOnlyCollection<string> mentionedPlayers)
        {
            CommandType = command;
            MentionedPlayers = mentionedPlayers;
        }
    }
}