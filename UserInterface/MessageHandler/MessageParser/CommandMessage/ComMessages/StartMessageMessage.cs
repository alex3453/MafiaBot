using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class StartMessageMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"start", "ыефке"};

        public override StartCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new StartCommandInfo(User, IsCommonChannel, CommonChannelId, Service);
        }

        public override string GetDescription() => "!start - позволяет начать игру.";
    }
}