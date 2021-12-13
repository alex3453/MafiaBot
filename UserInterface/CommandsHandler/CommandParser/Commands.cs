using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class HelpCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string> { "help", "рудз" };
        protected override ISet<string> PossibleStrings => _pos;
        protected override CommandType MyCommandType => CommandType.Help;

        public override string GetDescription()
        {
            return "команда !help";
        }
    }
}