namespace CommonInteraction
{
    public class User
    {
        public ulong Id { get; }
        public ulong ComChatId { get; }
        public string Name { get; }

        public User(ulong id, ulong comChatId, string name = "")
        {
            Id = id;
            ComChatId = comChatId;
            Name = name;
        }
    }
}