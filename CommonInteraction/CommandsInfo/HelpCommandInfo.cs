using System;

namespace CommonInteraction
{
    public class HelpCommandInfo : ICommandInfo
    {
        public HelpCommandInfo(User user, bool isComChat, ulong comChatId, Service service)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            Service = service;
        }
        
        public Service Service { get; }

        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong, Service> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}