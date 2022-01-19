using System;
using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class HelpMessage : AbstractViewComMessage
    {
        private readonly Func<MessageParser> _getParserFunc;
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string> { "help", "рудз" };
        
        public HelpMessage(IMessageSender[] senders, Func<MessageParser> getParserFunc) : base(senders)
        {
            _getParserFunc = getParserFunc;
        }
        
        public override void ExecuteCommand(MessageData msg)
        {
            if (!GetSender(msg.Service, out var sender)) return;
            var isCommonChannel = msg.IsCommonChannel;
            var channelId = isCommonChannel ? msg.CommonChannelId : msg.Author.Id;
            var parser = _getParserFunc();
            sender.SendMessage(new Answer(isCommonChannel, AnswerType.GetHelp, parser.GetCommandsDescription() ), channelId);
        }

        public override string GetDescription() => "!help - выведет данное приветственное сообщение и " +
                                                   "покажет все команды, если вы вдруг забыли.";
    }
}