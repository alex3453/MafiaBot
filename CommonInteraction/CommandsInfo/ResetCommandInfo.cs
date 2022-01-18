using System;

namespace CommonInteraction
{
    public class ResetCommandInfo : ICommandInfo
    {
        public ResetCommandInfo(User user, bool isComChat, ulong comChatId, string service)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            Service = service;
        }

        public string Service { get; }

        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong, string> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}