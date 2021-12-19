using System;

namespace CommonInteraction
{
    public class RegCommandInfo : ICommandInfo
    {
        public RegCommandInfo(User user, bool isComChat, ulong comChatId)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
        }

        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}