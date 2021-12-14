using System.Transactions;

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
        
        public override bool Equals(object obj) => obj is User otherUser && Equals(otherUser);
        public override int GetHashCode() => (Id, Name).GetHashCode();

        private bool Equals(User otherUser)
        {
            if (ReferenceEquals(this, otherUser))
                return true;
            if (otherUser == null)
                return false;
            return Id.Equals(otherUser.Id) &&
                   Name.Equals(otherUser.Name);
        }
    }
}