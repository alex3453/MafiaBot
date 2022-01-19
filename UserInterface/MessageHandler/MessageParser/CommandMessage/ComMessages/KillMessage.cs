using System.Collections.Generic;
using System.Linq;
using CommonInteraction;

namespace UserInterface
{
    public class KillMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"kill", "лшдд"};
        
        public override KillCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            var args = msg.Content.Split().Skip(1).ToArray();
            return new KillCommandInfo(User, IsCommonChannel, CommonChannelId, args, Service);
        }

        public override string GetDescription() => "!kill {номер игрока из отправленного вам списка} - " +
                                                   "позволяет мафии убивать игроков во время игры. " +
                                                   "Пишется только в личку боту.";
    }
}