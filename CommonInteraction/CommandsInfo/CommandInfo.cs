using System;

namespace CommonInteraction
{
    public interface ICommandInfo
    {
        public string Service { get; }
        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong, string> send);
        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
        
    }
}