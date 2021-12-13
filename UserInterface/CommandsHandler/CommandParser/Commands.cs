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

        public override string GetDescription() => "команда !help";
    }

    public class RegCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string> {"reg", "куп"};
        protected override ISet<string> PossibleStrings => _pos;
        protected override CommandType MyCommandType => CommandType.Reg;
        public override string GetDescription() => "команда !reg";
    }

    public class NewGameCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string> {"createnew", "скуфеутуц"};

        protected override ISet<string> PossibleStrings => _pos;

        protected override CommandType MyCommandType => CommandType.CreateNewGame;
        public override string GetDescription() => "команда !createnew";
    }
    
    public class StartCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string> {"start", "ыефке"};

        protected override ISet<string> PossibleStrings => _pos;

        protected override CommandType MyCommandType => CommandType.Start;
        public override string GetDescription() => "команда !start";
    }

    public class VoteCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string>{"vote", "мщеу"};

        protected override ISet<string> PossibleStrings => _pos;

        protected override CommandType MyCommandType => CommandType.Vote;
        public override string GetDescription() => "команда !vote";
    }

    public class KillCommand : Command
    {
        private readonly ISet<string> _pos = new HashSet<string> {"kill", "лшдд"};

        protected override ISet<string> PossibleStrings => _pos;

        protected override CommandType MyCommandType => CommandType.Kill;
        public override string GetDescription() => "команда !kill";
    }
}