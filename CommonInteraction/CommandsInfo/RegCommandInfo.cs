using App.CommandHandler;

namespace CommonInteraction
{
    public class RegCommandInfo : ICommandInfo
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Handle(this);
        }

        public RegCommandInfo(User user, bool isComChat, ulong comChatId)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}