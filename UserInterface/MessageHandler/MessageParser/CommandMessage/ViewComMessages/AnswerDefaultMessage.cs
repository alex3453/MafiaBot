using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class AnswerDefaultMessage : AbstractViewComMessage
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
}