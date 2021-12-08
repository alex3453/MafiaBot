using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafia
{
    public class MafiaGame : IMafia
    {
        private readonly List<Player> allPlayers = new();
        private readonly List<Player> playersInGame = new();
        private readonly List<Player> deadPlayers = new();
        private readonly List<Player> kickedPlayers = new();
        private readonly List<Player> mafiozyPlayers = new();
        private List<Player> toKillPlayers = new();
        private List<Player> votedPlayers = new();
        private readonly List<Role> roles = new();
        public Player Dead { get; private set; }


        public Status Status { get; private set; }
        public int NightNumber { get; }

        

        public bool Vote(Player voter, Player target)
        {
            if (votedPlayers.Contains(voter))
                return false;
            votedPlayers.Add(voter);
            target.VoteMe();
            if (allPlayers.Sum(player => player.VoteCount) == allPlayers.Count)
                EndDay();
            return true;
        }

        // public Answer Kill(Player killer, Player target)
        // {
        //     if (toKillPlayers.Contains(killer))
        //         return new Answer(true, AnswerType.UnsuccessfullyKill);
        //     toKillPlayers.Add(killer);
        //     target.KillMe();
        //     return mafiozyPlayers.Sum(player => player.KillCount) == mafiozyPlayers.Count ? 
        //         EndNight() : 
        //         new Answer(true, AnswerType.SuccessfullyKill);
        // }

        public void RegisterPlayer(Player player)
        {
            allPlayers.Add(player);
            Status = Status.ReadyToStart;
        }
        
        
        public void StartGame()
        {
            var mafiaCount = allPlayers.Count / 5 + 1;
            for (var i = 0; i < mafiaCount; i++)
                roles.Add(new MafiaRole());
            for (var i = 0; i < allPlayers.Count - mafiaCount; i++)
                roles.Add(new PeacefulRole());

            var random = new Random();
            for (var i = roles.Count - 1; i >= 1; i--)
            {
                var j = random.Next(i + 1);
                (roles[j], roles[i]) = (roles[i], roles[j]);
            }

            for (var i = 0; i < roles.Count; i++)
            {
                allPlayers[i].SetRole(roles[i]);
                if (roles[i] is MafiaRole)
                    mafiozyPlayers.Add(allPlayers[i]);
                playersInGame.Add(allPlayers[i]);
            }

            Status = Status.Voting;
        }

        public List<Player> Winners { get; }
        


        public void EndDay()
        {
            votedPlayers = new List<Player>();
            var died = allPlayers.OrderByDescending(x => x.VoteCount).First();
            deadPlayers.Add(died);
            Dead = died;
            if (died.Role is MafiaRole)
                mafiozyPlayers.Remove(died);
            CheckWin(Status.Night);
        }

        public void EndNight()
        {
            toKillPlayers = new List<Player>();
            var died = allPlayers.OrderByDescending(x => x.KillCount).First();
            deadPlayers.Add(died);
            Dead = died;
            if (died.Role is MafiaRole)
                mafiozyPlayers.Remove(died);
            CheckWin(Status.Day);
            Status = Status.Day;
        }

        private void CheckWin(Status nextStep)
        {
            if (mafiozyPlayers.Count == 0)
                Status = Status.PeacefulWins;
            else if (mafiozyPlayers.Count >= allPlayers.Count / 2.0)
                Status = Status.MafiaWins;
            else
                Status = nextStep;
        }
        public IReadOnlyList<Player> GetAllPlayers => allPlayers;
        public IReadOnlyCollection<Player> GetPlayersInGame => playersInGame;
        public IReadOnlyCollection<Player> GetDeadPlayers => deadPlayers;
        public IReadOnlyCollection<Player> GetKickedPlayers => kickedPlayers;
        public IReadOnlyList<Role> GetRoles => roles;
    }
}
