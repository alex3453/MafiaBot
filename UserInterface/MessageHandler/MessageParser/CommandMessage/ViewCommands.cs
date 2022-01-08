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

        public override void ExecuteCommand(MessageData msg)
        {
            if (!msg.IsCommonChannel) return;
            _sender.SetParser(_default);   
            _sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Обычненька"), msg.CommonChannelId);
        }

        public override string GetDescription() => "!default - обычный решим ответов. " +
                                                   "Бот будет оповещать о событиях заготовленными фразами";
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

        public override void ExecuteCommand(MessageData msg)
        {
            if (!msg.IsCommonChannel) return;
            _sender.SetParser(_parser);   
            _sender.SendMessage(new Answer(true, AnswerType.ChangeMod, "Балабобненька"), msg.CommonChannelId);
        }

        public override string GetDescription() =>
            "!balaboba - поменяет ответы бота. Данный режим реализован с помощью" +
            "Балабобы от Яндекса - нейросеть, которая дополняет ответы." +
            "Будьте бдительны! Если в вашем нике есть что-то, связанное с политикой," +
            "то Балабоба вам не ответит:(";
    }
    
    public class HelpMessage : ViewCommandMessage
    {
        private readonly Func<IMessageParser> getParser;
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        public override void ExecuteCommand(MessageData msg)
        {
            var isCommonChannel = msg.IsCommonChannel;
            var channelId = isCommonChannel ? msg.CommonChannelId : msg.Author.Id;
            var parser = getParser();
            _sender.SendMessage(new Answer(isCommonChannel, AnswerType.GetHelp, parser.GetCommandsDescription() ), channelId);
        }

        public override string GetDescription() => "!help - выведет данное приветственное сообщение и " +
                                                   "покажет все команды, если вы вдруг забыли.";

        public HelpMessage(IMessageSender sender, Func<IMessageParser> getHelp) : base(sender)
        {
            getParser = getHelp;
        }
    }
}