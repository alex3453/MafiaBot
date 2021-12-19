using System;

namespace CommonInteraction
{
    public interface ICommandInfo
    {
        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong> send);
        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
        
    }
}