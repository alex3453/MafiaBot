using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;

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

        public Status Status { get; private set; }
        public int NightNumber { get; }
        
        public Answer Vote(Player voter, Player target)
        {
            if (votedPlayers.Contains(voter))
                return new Answer(true, AnswerType.UnsuccessfullyVoted);
            votedPlayers.Add(voter);
            target.VoteMe();
            return allPlayers.Sum(player => player.VoteCount) == allPlayers.Count ? 
                EndDay() : 
                new Answer(true, AnswerType.SuccessfullyVoted);
        }

        public Answer Kill(Player killer, Player target)
        {
            if (toKillPlayers.Contains(killer))
                return new Answer(true, AnswerType.UnsuccessfullyKill);
            toKillPlayers.Add(killer);
            target.KillMe();
            return mafiozyPlayers.Sum(player => player.KillCount) == mafiozyPlayers.Count ? 
                EndNight() : 
                new Answer(true, AnswerType.SuccessfullyKill);
        }

        public void RegisterPlayer(Player player)
        {
            allPlayers.Add(player);
            Status = Status.ReadyToStart;
        }
        
        
        public Answer StartGame()
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
            return new Answer(true, AnswerType.GameStart);
        }

        public List<Player> Winners { get; }
        
        public Answer GetRules()
        {
            return new Answer(true, AnswerType.GetRules);
        }
        
        public Answer EndDay()
        {
            votedPlayers = new List<Player>();
            var died = allPlayers.OrderByDescending(x => x.VoteCount).First();
            deadPlayers.Add(died);
            if (died.Role is MafiaRole)
                mafiozyPlayers.Remove(died);
            Status = Status.Night;
            if (mafiozyPlayers.Count == 0)
                return new Answer(true, AnswerType.PeacefulWins, new List<string>{died.Name});
            if (mafiozyPlayers.Count >= allPlayers.Count / 2.0)
                return new Answer(true, AnswerType.MafiaWins, new List<string>{died.Name});
            return new Answer(true, AnswerType.EndDay, new List<string>{died.Name, died.Role.ToString()});
        }

        public Answer EndNight()
        {
            toKillPlayers = new List<Player>();
            var died = allPlayers.OrderByDescending(x => x.KillCount).First();
            deadPlayers.Add(died);
            if (died.Role is MafiaRole)
                mafiozyPlayers.Remove(died);
            Status = Status.Day;
            if (mafiozyPlayers.Count == 0)
                return new Answer(true, AnswerType.PeacefulWins, new List<string>{died.Name});
            if (mafiozyPlayers.Count >= allPlayers.Count / 2.0)
                return new Answer(true, AnswerType.MafiaWins, new List<string>{died.Name});
            return new Answer(true, AnswerType.EndNight, new List<string>{died.Name, died.Role.ToString()});
        }
        public IReadOnlyList<Player> GetAllPlayers => allPlayers;
        public IReadOnlyCollection<Player> GetPlayersInGame => playersInGame;
        public IReadOnlyCollection<Player> GetDeadPlayers => deadPlayers;
        public IReadOnlyCollection<Player> GetKickedPlayers => kickedPlayers;
        public IReadOnlyList<Role> GetRoles => roles;
    }
}
