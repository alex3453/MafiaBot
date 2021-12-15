using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Mafia
{
    public class MafiaGame : IMafia
    {
        private readonly List<Player> allPlayers = new();
        private readonly List<Player> playersInGame = new();
        private readonly HashSet<Player> deadPlayers = new();
        private readonly List<Player> mafiozyPlayers = new();
        private HashSet<string> votedPlayers = new();
        private readonly Dictionary<int, string> playersNumbers = new();
        private readonly IRoleDistribution roleDist;
        
        public bool IsSomeBodyDied { get; private set; }

        public Status Status { get; private set; } = Status.WaitingPlayers;

        public MafiaGame(IRoleDistribution roleDist)
        {
            this.roleDist = roleDist;
        }

        public OperationStatus RegisterPlayer(string name)
        {
            var player = new Player(name);
            if (allPlayers.Contains(player)) return OperationStatus.Already;
            allPlayers.Add(player);
            if (allPlayers.Count >= 4) Status = Status.ReadyToStart;
            return OperationStatus.Success;
        }

        public void StartGame()
        {
            var roles = roleDist.DistributeRoles(allPlayers.Count);
            for (var i = 0; i < roles.Count; i++)
            {
                allPlayers[i].SetRole(roles[i]);
                if (roles[i] is MafiaRole)
                    mafiozyPlayers.Add(allPlayers[i]);
                playersInGame.Add(allPlayers[i]);
                playersNumbers[i + 1] = allPlayers[i].Name;
            }
            Status = Status.Voting;
        }
        

        private void EndDay()
        {
            deadPlayers.Clear();
            IsSomeBodyDied = false;
            votedPlayers = new HashSet<string>();
            var deadP = playersInGame.OrderByDescending(x => x.VoteCount).First();
            if (deadP.VoteCount >= playersInGame.Count / 2.0)
            {
                IsSomeBodyDied = true;
                KillPlayer(deadP);
            }

            foreach (var player in playersInGame)
                player.ResetVoteCount();
            Status = Status.MafiaKilling;
            CheckWin();
        }

        private void EndNight()
        {
            deadPlayers.Clear();
            foreach (var player in allPlayers.Where(player => player.Role is MafiaRole))
                player.Role.act = false;
            var deadP = playersInGame.OrderByDescending(x => x.KillCount).First();
            foreach (var player in playersInGame)
                player.ResetKillCount();
            IsSomeBodyDied = true;
            KillPlayer(deadP);
            Status = Status.Voting;
            CheckWin();
        }
        
        public OperationStatus Vote(string voter, string target)
        {
            if (playersInGame.All(x => x.Name != voter))
                return OperationStatus.NotInGame;
            if (votedPlayers.Contains(voter))
                return OperationStatus.Already;
            var targetP = playersInGame.FirstOrDefault(x => x.Name == target);
            if (targetP is null)
                return OperationStatus.Cant;
            votedPlayers.Add(voter);
            targetP.VoteMe();
            if (playersInGame.Sum(player => player.VoteCount) == playersInGame.Count)
                EndDay();
            return OperationStatus.Success;
        }

        public OperationStatus Act(Player maker, int target)
        {
            if (playersInGame.All(x => x.Name != maker.Name))
                return OperationStatus.NotInGame;
            if (target > playersInGame.Count || target <= 0)
                return OperationStatus.Incorrect;
            var actStatus = maker.Role.Act(target, playersInGame);
            switch (actStatus)
            {
                case ActStatus.Already:
                    return OperationStatus.Already;
                case ActStatus.EndNight:
                    EndNight();
                    break;
            }

            return OperationStatus.Success;
        }

        private void KillPlayer(Player deadP)
        {
            deadPlayers.Add(deadP);
            playersInGame.Remove(deadP);
            mafiozyPlayers.Remove(deadP);
        }

        private void CheckWin()
        {
            if (mafiozyPlayers.Count == 0)
                Status = Status.PeacefulWins;
            else if (mafiozyPlayers.Count >= playersInGame.Count / 2.0)
                Status = Status.MafiaWins;
        }

        public IReadOnlyCollection<string> GetWinners()
        {
            if (Status == Status.PeacefulWins)
            {
                return allPlayers
                    .Where(p => p.Role is PeacefulRole)
                    .Select(p => p.Name).ToArray();
            }
            if (Status == Status.MafiaWins)
            {
                return allPlayers
                    .Where(p => p.Role is MafiaRole)
                    .Select(p => p.Name).ToArray();
            }

            return Array.Empty<string>();
        }

        public IReadOnlyCollection<Player> AllPlayers => allPlayers;
        public IReadOnlyCollection<string> PlayersInGame => playersInGame.Select(p => p.Name).ToArray();
        public IReadOnlyDictionary<string, Role> PlayersRoles => allPlayers.ToDictionary(p => p.Name, p => p.Role);
        public IReadOnlyCollection<string> MafiozyPlayers => mafiozyPlayers.Select(p => p.Name).ToArray();
        public IReadOnlyCollection<string> Dead => deadPlayers.Select(p => p.Name).ToArray();
        public IReadOnlyDictionary<int, string> PlayersNumbers => playersNumbers;
        public IReadOnlyCollection<string> DeadPlayers => deadPlayers.Select(p => p.Name).ToArray();
    }
}
