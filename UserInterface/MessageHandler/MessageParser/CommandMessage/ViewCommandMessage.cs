using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public abstract class ViewCommandMessage
    {
        protected readonly IMessageSender _sender;
        protected ViewCommandMessage(IMessageSender sender)
        {
            _sender = sender;
        }
        protected abstract ISet<string> PossibleStrings { get; }
        public bool IsItMyCommand(SocketMessage msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }

        public abstract void ExecuteCommand(SocketMessage msg);
        public abstract string GetDescription();
    }
}