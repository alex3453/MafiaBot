using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class AnswerBalabobaMessage : AbstractViewComMessage
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
}