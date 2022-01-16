using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public abstract class ViewCommandMessage
    {
        protected readonly IMessageSender[] _senders;
        protected ViewCommandMessage(IMessageSender[] sender)
        {
            _senders = sender;
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

        protected bool GetSender(Service service, out IMessageSender sender)
        {
            sender = _senders.FirstOrDefault(s => s.IsItMyService(service));
            return sender != null;
        }
    }
}