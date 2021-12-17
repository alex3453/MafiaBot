namespace Mafia
{
    public class Player
    {
        public string Name { get; }
        
        public bool IsSkipsMove { get; private set; }
        public int VoteCount { get; private set; }
        public int KillCount { get; private set; }

        public Role Role { get; private set; }
        
        public Player(string name)
        {
            Name = name;
            VoteCount = 0;
        }

        public void SetRole(Role role)
        {
            Role = role;
        }

        public void ResetVoteCount()
        {
            VoteCount = 0;
        }

        public void ResetKillCount()
        {
            KillCount = 0;
        }

        public void VoteMe()
        {
            VoteCount++;
        }

        public void KillMe()
        {
            KillCount++;
        }
    }
}