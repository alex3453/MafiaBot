namespace CommonInteraction
{
    public class User
    {
        public ulong Id { get; }
        public ulong CommonChannelId { get; }
        public string Name { get; }

        public User(ulong id, ulong commonChannelId, string name = "")
        {
            Id = id;
            CommonChannelId = commonChannelId;
            Name = name;
        }
    }
}