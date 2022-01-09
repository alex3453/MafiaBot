using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class RegMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"reg", "куп"};

        public override RegCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new RegCommandInfo(User, IsCommonChannel, CommonChannelId);
        }


        public override string GetDescription() => "!reg - позволяет зарегестрироваться на игру.";
    }

    public class ResetGameMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"createnew", "скуфеутуц"};

        public override ResetCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new ResetCommandInfo(User, IsCommonChannel, CommonChannelId);
        }

        public override string GetDescription() => "!createnew - создает для вас новую игру.";
    }
    
    public class StartMessageMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"start", "ыефке"};

        public override StartCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            return new StartCommandInfo(User, IsCommonChannel, CommonChannelId);
        }

        public override string GetDescription() => "!start - позволяет начать игру.";
    }

    public class VoteMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string>{"vote", "мщеу"};
        public override VoteCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            var mentionedUsers = msg.MentionedUsers;
            return new VoteCommandInfo(User, IsCommonChannel, CommonChannelId, mentionedUsers);
        }

        public override string GetDescription() => "!vote {имя игрока на сервере, лучше через @} - " +
                                                   "позволяет голосовать во время самой игры.";
    }

    public class KillMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"kill", "лшдд"};
        public override KillCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            var args = msg.Content.Split().Skip(1).ToArray();
            return new KillCommandInfo(User, IsCommonChannel, CommonChannelId, args);
        }

        public override string GetDescription() => "!kill {номер игрока из отправленного вам списка} - " +
                                                   "позволяет мафии убивать игроков во время игры. " +
                                                    "Пишется только в личку боту.";
    }
}