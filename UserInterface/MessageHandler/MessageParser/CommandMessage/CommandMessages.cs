using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class HelpMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        public override ICommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            return new HelpCommandInfo(user, isCommonChannel, commonChannelId);
        }

        public override string GetDescription() => "!help - выведет данное приветственное сообщение и " +
                                                   "покажет все команды, если вы вдруг забыли.";
    }

    public class RegMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"reg", "куп"};

        public override RegCommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            return new RegCommandInfo(user, isCommonChannel, commonChannelId);
        }


        public override string GetDescription() => "!reg - позволяет зарегестрироваться на игру.";
    }

    public class ResetGameMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"createnew", "скуфеутуц"};

        public override ResetCommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            return new ResetCommandInfo(user, isCommonChannel, commonChannelId);
        }

        public override string GetDescription() => "!createnew - создает для вас новую игру.";
    }
    
    public class StartMessageMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"start", "ыефке"};

        public override StartCommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            return new StartCommandInfo(user, isCommonChannel, commonChannelId);
        }

        public override string GetDescription() => "!start - позволяет начать игру.";
    }

    public class VoteMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string>{"vote", "мщеу"};
        public override VoteCommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            var mentionedUsers = msg.MentionedUsers.Select(x => x.Username).ToArray();
            return new VoteCommandInfo(user, isCommonChannel, commonChannelId, mentionedUsers);
        }

        public override string GetDescription() => "!vote {имя игрока на сервере, лучше через @} - " +
                                                   "позволяет голосовать во время самой игры.";
    }

    public class KillMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"kill", "лшдд"};
        public override KillCommandInfo GetCommandInfo(SocketMessage msg)
        {
            FillCommonInfo(msg);
            var args = msg.Content.Split().Skip(1).ToArray();
            return new KillCommandInfo(user, isCommonChannel, commonChannelId, args);
        }

        public override string GetDescription() => "!kill {номер игрока из отправленного вам списка} - " +
                                                   "позволяет мафии убивать игроков во время игры. " +
                                                    "Пишется только в личку боту.";
    }
}