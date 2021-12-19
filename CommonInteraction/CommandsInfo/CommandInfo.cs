using System;

namespace CommonInteraction
{
    public interface ICommandInfo
    {
        public void Accept(IVisitor visitor, Action<Answer, ulong> send);
        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
        
    }
}