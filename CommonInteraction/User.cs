namespace CommonInteraction
{
    public class User
    {
        public ulong Id { get; }
        public ulong ComChatId { get; }
        public bool IsCommonChat { get; }
        public string Name { get; }

        public User(ulong id, ulong comChatId, bool isCommonChat, string name = "")
        {
            Id = id;
            ComChatId = comChatId;
            IsCommonChat = isCommonChat;
            Name = name;
        }
    }
}