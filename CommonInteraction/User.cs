namespace CommonInteraction
{
    public class User
    {
        public ulong Id { get; }
        public bool IsChannel { get; }
        public string Name { get; }

        public User(ulong id, bool isChannel, string name = "")
        {
            Id = id;
            IsChannel = isChannel;
            Name = name;
        }
    }
}