using System.Collections.Generic;

namespace CommonInteraction
{
    public interface ICommandInfo
    {
        public void Accept(IVisitor visitor);
        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
        
    }
}