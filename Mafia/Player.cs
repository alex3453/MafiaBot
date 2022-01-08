namespace Mafia
{
    public class Player
    {
        public string Name { get; }
        
        public bool IsSkipsMove { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public int VoteCount { get; private set; } = 0;

        public Role Role { get; private set; }
        public int KillCount { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public void SetRole(Role role)
        {
            Role = role;
        }

        public void VoteMe() => VoteCount++;
        public void KillMe() => KillCount++;
        public void HealMe() => IsAlive = true;
        public void ResetVoteCount() => VoteCount = 0;

        public void ResetKillCount() => KillCount = 0;
    }
}