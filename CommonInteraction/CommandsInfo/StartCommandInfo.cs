namespace CommonInteraction
{
    public class StartCommandInfo : ICommandInfo
    {

        public StartCommandInfo(User user, bool isComChat, ulong comChatId)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Handle(this);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}