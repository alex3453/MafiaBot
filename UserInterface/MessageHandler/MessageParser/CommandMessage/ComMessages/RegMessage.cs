using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class RegMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"reg", "куп"};

        public override RegCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new RegCommandInfo(User, IsCommonChannel, CommonChannelId, Service);
        }
        
        public override string GetDescription() => "!reg - позволяет зарегестрироваться на игру.";
    }
}