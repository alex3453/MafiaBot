using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class ResetGameMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"createnew", "скуфеутуц"};

        public override ResetCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new ResetCommandInfo(User, IsCommonChannel, CommonChannelId, Service);
        }

        public override string GetDescription() => "!createnew - создает для вас новую игру.";
    }
}