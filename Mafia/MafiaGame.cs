using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscordBot.Mafia;

namespace DiscordBot
{
    class MafiaGame : IMafia
    {
        private readonly List<Player> allPlayers = new List<Player>();
        private readonly List<Player> playersInGame = new List<Player>();
        private readonly List<Player> deadPlayers = new List<Player>();
        private readonly List<Player> kickedPlayers = new List<Player>();
        private readonly List<Player> mafiozyPlayers = new List<Player>();
        private List<Player> toKillPlayers = new List<Player>();
        private List<Player> votedPlayers = new List<Player>();
        private readonly List<Role> roles = new List<Role>();

        public Status Status { get; private set; }
        public int NightNumber { get; }
        public Answer Vote(Player voter, Player target)
        {
            if (votedPlayers.Contains(voter))
                return new Answer(true, Answers.UnsuccessfullyVoted);
            votedPlayers.Add(voter);
            target.VoteMe();
            return allPlayers.Sum(player => player.VoteCount) == allPlayers.Count ? 
                EndDay() : 
                new Answer(true, Answers.SuccessfullyVoted);
        }

        public Answer Kill(Player killer, Player target)
        {
            if (toKillPlayers.Contains(killer))
                return new Answer(true, Answers.UnsuccessfullyKill);
            toKillPlayers.Add(killer);
            target.KillMe();
            return mafiozyPlayers.Sum(player => player.KillCount) == mafiozyPlayers.Count ? 
                EndNight() : 
                new Answer(true, Answers.SuccessfullyKill);
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
            return new Answer(true, Answers.GameStart);
        }

        public bool IsPlayerInGame(ulong id)
        {
            throw new NotImplementedException();
        }

        public bool PerformAction(Player author, object[] args)
        {
            throw new NotImplementedException();
        }

        public List<Player> Winners { get; }
        
        public Answer GetRules()
        {
            return new Answer(true, Answers.GetRules);
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
                return new Answer(true, Answers.PeacefulWins, new List<string>{died.Name});
            if (mafiozyPlayers.Count >= allPlayers.Count / 2.0)
                return new Answer(true, Answers.MafiaWins, new List<string>{died.Name});
            return new Answer(true, Answers.EndDay, new List<string>());
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
                return new Answer(true, Answers.PeacefulWins, new List<string>{died.Name});
            if (mafiozyPlayers.Count >= allPlayers.Count / 2.0)
                return new Answer(true, Answers.MafiaWins, new List<string>{died.Name});
            return new Answer(true, Answers.EndNight, new List<string>());
        }

        public IReadOnlyList<Player> GetAllPlayers => allPlayers;
        public IReadOnlyCollection<Player> GetPlayersInGame => playersInGame;
        public IReadOnlyCollection<Player> GetDeadPlayers => deadPlayers;
        public IReadOnlyCollection<Player> GetKickedPlayers => kickedPlayers;
        public IReadOnlyList<Role> GetRoles => roles;
    }
}
