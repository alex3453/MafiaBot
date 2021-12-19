using System;

namespace CommonInteraction
{
    public interface ICommandInfo
    {
        public THandler Accept<THandler>(IVisitor<THandler> visitor, Action<Answer, ulong> send);
        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
        
    }
}