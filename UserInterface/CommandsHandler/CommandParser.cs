using CommonInteraction;

namespace UserInterface
{
    public class CommandParser
    {
        private string command;

        public CommandParser(string command)
        {
            this.command = command;
        }

        public CommandType Parse()
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