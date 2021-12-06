namespace DiscordBot.Mafia
{
    public class Player
    {
        public string Name { get; }
        public bool IsAlive { get; private set; }
        public bool IsSkipsMove { get; private set; }
        public int VoteCount { get; private set; }
        public int KillCount { get; private set; }
        public void Kill() => IsAlive = false;
        
        public Role Role { get; private set; }
        
        public Player(string name)
        {
            Name = name;
            IsAlive = true;
            VoteCount = 0;
        }
        
        public Player(string name, Role role)
        {
            Name = name;
            IsAlive = true;
            VoteCount = 0;
            Role = role;
        }

        public void SetRole(Role role)
        {
            Role = role;
        }

        public void ResetVoteCount()
        {
            VoteCount = 0;
        }

        public void VoteMe()
        {
            VoteCount++;
        }

        public void KillMe()
        {
            KillCount++;
        }

        public void BanMoving() => IsSkipsMove = true;
        public void Unban() => IsSkipsMove = false;
    }
}