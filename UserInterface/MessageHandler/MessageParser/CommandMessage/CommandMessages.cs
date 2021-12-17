using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class HelpMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        public override ICommandInfo GetCommandInfo() => new HelpCommandInfo(user, isCommonChannel, commonChannelId);
        public override string GetDescription() => "команда !help";
    }

    public class RegMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"reg", "куп"};

        public override RegCommandInfo GetCommandInfo() => new(user, isCommonChannel, commonChannelId);


        public override string GetDescription() => "команда !reg";
    }

    public class ResetGameMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"createnew", "скуфеутуц"};
        public override ResetCommandInfo GetCommandInfo() => new(user, isCommonChannel, commonChannelId);

        public override string GetDescription() => "команда !createnew";
    }
    
    public class StartMessageMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"start", "ыефке"};
        public override StartCommandInfo GetCommandInfo() => new(user, isCommonChannel, commonChannelId);

        public override string GetDescription() => "команда !start";
    }

    public class VoteMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string>{"vote", "мщеу"};
        public override VoteCommandInfo GetCommandInfo() => new(user, isCommonChannel, commonChannelId, mentionedUsers);

        public override string GetDescription() => "команда !vote";
    }

    public class KillMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"kill", "лшдд"};
        public override KillCommandInfo GetCommandInfo() => new(user, isCommonChannel, commonChannelId, args);

        public override string GetDescription() => "команда !kill";
    }
}