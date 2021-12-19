using System;

namespace CommonInteraction
{
    public class ResetCommandInfo : ICommandInfo
    {
        public ResetCommandInfo(User user, bool isComChat, ulong comChatId)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
        }

        public THandler Accept<THandler>(IVisitor<THandler> visitor, Action<Answer, ulong> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}