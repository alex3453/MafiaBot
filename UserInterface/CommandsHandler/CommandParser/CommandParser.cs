using CommonInteraction;

namespace UserInterface
{
    public class CommandParser : ICommandParser
    {
        public CommandType Parse(string command)
        {
            switch (command)
            {
                case "help":
                case "рудз":
                    return CommandType.Help;
                case "vote":
                case "мщеу":
                    return CommandType.Vote;
                case "reg":
                case "куп":
                    return CommandType.Reg;
                case "kill":
                case "лшдд":
                    return CommandType.Kill;
                case "start":
                case "ыефке":
                    return CommandType.Start;
                case "createnew":
                case "скуфеутуц":
                    return CommandType.CreateNewGame;
                default:
                    return CommandType.Unknown;
            }
        }
    }
}