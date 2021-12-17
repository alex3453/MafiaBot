using System.Collections.Generic;
using App.CommandHandler;

namespace CommonInteraction
{
    public class KillCommandInfo : ICommandInfo
    {
        public readonly IEnumerable<string> Content;
        
        public void Accept(IVisitor visitor)
        {
            visitor.Handle(this);
        }
        public KillCommandInfo(User user, bool isComChat, ulong comChatId, IEnumerable<string> content)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            Content = content;
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}