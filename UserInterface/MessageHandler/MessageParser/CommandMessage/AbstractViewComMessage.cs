using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public abstract class AbstractViewComMessage
    {
        protected readonly IMessageSender[] Senders;
        protected AbstractViewComMessage(IMessageSender[] sender)
        {
            Senders = sender;
        }
        
        protected abstract ISet<string> PossibleStrings { get; }
        
        public bool IsItMyCommand(MessageData msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }
        
        public abstract void ExecuteCommand(MessageData msg);
        public abstract string GetDescription();

        protected bool GetSender(string service, out IMessageSender sender)
        {
            sender = Senders.FirstOrDefault(s => s.IsItMyService(service));
            return sender != null;
        }
    }
}