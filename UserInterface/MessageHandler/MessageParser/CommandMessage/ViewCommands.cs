using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class AnswerDefaultMessage : ViewCommandMessage
    {
        private readonly IMessageSender _sender;
        private readonly DefaultParser _default;
        public AnswerDefaultMessage(IMessageSender sender, DefaultParser defaultParser) : base(sender)
        {
            _sender = sender;
            _default = defaultParser;
        }
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"default", "вуафгде", "обычный"};

        public override void ExecuteCommand(SocketMessage msg)
        {
            if (!(msg.Channel.GetType() == typeof(SocketTextChannel))) return;
            _sender.SetParser(_default);   
            _sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Обычненька"), msg.Channel.Id);
        }

        public override string GetDescription() => "!default";
    }
    
    public class AnswerBalabobaMessage : ViewCommandMessage
    {
        private readonly IMessageSender _sender;
        private readonly BalabobaParser _parser;

        public AnswerBalabobaMessage(IMessageSender sender, BalabobaParser parser) : base(sender)
        {
            _sender = sender;
            _parser = parser;
        }

        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"balaboba", "ифдфищиф", "балабоба"};

        public override void ExecuteCommand(SocketMessage msg)
        {
            if (!(msg.Channel.GetType() == typeof(SocketTextChannel))) return;
            _sender.SetParser(_parser);   
            _sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Балабобненька"), msg.Channel.Id);
        }

        public override string GetDescription() =>
            "!balaboba - поменяет ответы бота. Данный режим реализован с помощью" +
            "Балабобы от Яндекса - нейросеть, которая дополняет ответы." +
            "Будьте бдительны! Если в вашем нике есть что-то, связанное с политикой," +
            "то Балабоба вам не ответит:(";
    }
    
    public class HelpMessage : ViewCommandMessage
    {
        private readonly Func<string> _getHelp;
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        public override void ExecuteCommand(SocketMessage msg)
        {
            var isCommonChannel = msg.Channel.GetType() == typeof(SocketTextChannel);
            var channelId = isCommonChannel ? msg.Channel.Id : msg.Author.Id;
            _sender.SendMessage(new Answer(isCommonChannel, AnswerType.GetHelp, _getHelp()), channelId);
        }

        public override string GetDescription() => "!help - выведет данное приветственное сообщение и " +
                                                   "покажет все команды, если вы вдруг забыли.";

        public HelpMessage(IMessageSender sender, Func<string> getHelp) : base(sender)
        {
            _getHelp = getHelp;
        }
    }
}