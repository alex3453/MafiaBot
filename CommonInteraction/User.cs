namespace CommonInteraction
{
    public class User
    {
        public readonly ulong Id;
        public readonly ulong CommonChannelId;
        public readonly string Name;

        public User(ulong id, ulong commonChannelId, string name = "")
        {
            Id = id;
            CommonChannelId = commonChannelId;
            Name = name;
        }
    }
}