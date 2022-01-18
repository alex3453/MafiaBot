namespace CommonInteraction
{
    public class User
    {
        public readonly ulong Id;
        public readonly string Name;

        public User(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
        public override int GetHashCode() => (Id, Name).GetHashCode();
        
        public override bool Equals(object obj) => obj is User otherUser && Equals(otherUser);
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