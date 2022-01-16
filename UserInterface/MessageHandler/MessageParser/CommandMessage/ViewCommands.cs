using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class AnswerDefaultMessage : ViewCommandMessage
    {
        private readonly DefaultGenerator _default;
        public AnswerDefaultMessage(IMessageSender[] senders, DefaultGenerator defaultGenerator) : base(senders)
        {
            _default = defaultGenerator;
        }
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"default", "вуафгде", "обычный"};

        public override void ExecuteCommand(MessageData msg)
        {
            if (!msg.IsCommonChannel || !GetSender(msg.Service, out var sender)) return;
            sender.SetParser(_default);   
            sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Обычненька"), msg.CommonChannelId);
        }

        public override string GetDescription() => "!default - обычный решим ответов. " +
                                                   "Бот будет оповещать о событиях заготовленными фразами";
    }
    
    public class AnswerBalabobaMessage : ViewCommandMessage
    {
        private readonly BalabobaGenerator _generator;

        public AnswerBalabobaMessage(IMessageSender[] senders, BalabobaGenerator generator) : base(senders)
        {
            _generator = generator;
        }

        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> {"balaboba", "ифдфищиф", "балабоба"};

        public override void ExecuteCommand(MessageData msg)
        {
            if (!msg.IsCommonChannel || !GetSender(msg.Service, out var sender)) return;
            sender.SetParser(_generator);   
            sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Балабобненька"), msg.CommonChannelId);
        }

        public override string GetDescription() =>
            "!balaboba - поменяет ответы бота. Данный режим реализован с помощью" +
            "Балабобы от Яндекса - нейросеть, которая дополняет ответы." +
            "Будьте бдительны! Если в вашем нике есть что-то, связанное с политикой," +
            "то Балабоба вам не ответит:(";
    }
    
    public class HelpMessage : ViewCommandMessage
    {
        private readonly Func<MessageParser> getParser;
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        
        public HelpMessage(IMessageSender[] senders, Func<MessageParser> getParser) : base(senders)
        {
            this.getParser = getParser;
        }
        public override void ExecuteCommand(MessageData msg)
        {
            if (!GetSender(msg.Service, out var sender)) return;
            var isCommonChannel = msg.IsCommonChannel;
            var channelId = isCommonChannel ? msg.CommonChannelId : msg.Author.Id;
            var parser = getParser();
            sender.SendMessage(new Answer(isCommonChannel, AnswerType.GetHelp, parser.GetCommandsDescription() ), channelId);
        }

        public override string GetDescription() => "!help - выведет данное приветственное сообщение и " +
                                                   "покажет все команды, если вы вдруг забыли.";
    }
}