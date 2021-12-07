using System.Collections.Generic;


namespace Commands
{
    public enum CommandType
    {
        VoteCommand,
        RulesCommand,
        StartCommand,
        RegCommand,
        KillCommand,
        None
    }
    public class Command
    {
        public CommandType CommandType { get; }
        public string AuthorName { get; }
        public IReadOnlyCollection<string> MentionedPlayers { get; }

        public Command(CommandType command, IReadOnlyCollection<string> mentionedPlayers, string authorName)
        {
            CommandType = command;
            MentionedPlayers = mentionedPlayers;
            AuthorName = authorName;
        }
    }
}