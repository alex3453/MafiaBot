using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafia
{
    public class MafiaGame : IMafia
    {
        private readonly List<Player> _allPlayers = new();
        private readonly List<Player> _playersInGame = new();
        private readonly HashSet<Player> _deadPlayers = new();
        private readonly List<Player> _mafiozyPlayers = new();
        private HashSet<string> _votedPlayers = new();
        private readonly Dictionary<int, string> _playersNumbers = new();
        private readonly IRoleDistribution _roleDist;
        
        public bool IsSomeBodyDied { get; private set; }

        public Status Status { get; private set; } = Status.WaitingPlayers;

        public MafiaGame(IRoleDistribution roleDist)
        {
            this._roleDist = roleDist;
        }

        public OperationStatus RegisterPlayer(string name)
        {
            var player = new Player(name);
            if (_allPlayers.Contains(player)) return OperationStatus.Already;
            _allPlayers.Add(player);
            if (_allPlayers.Count >= 4) Status = Status.ReadyToStart;
            return OperationStatus.Success;
        }

        public void StartGame()
        {
            var roles = _roleDist.DistributeRoles(_allPlayers.Count);
            for (var i = 0; i < roles.Count; i++)
            {
                _allPlayers[i].SetRole(roles[i]);
                if (roles[i] is MafiaRole)
                    _mafiozyPlayers.Add(_allPlayers[i]);
                _playersInGame.Add(_allPlayers[i]);
                _playersNumbers[i + 1] = _allPlayers[i].Name;
            }
            Status = Status.Voting;
        }
        

        private void EndDay()
        {
            _deadPlayers.Clear();
            IsSomeBodyDied = false;
            _votedPlayers = new HashSet<string>();
            var deadP = _playersInGame.OrderByDescending(x => x.VoteCount).First();
            if (deadP.VoteCount >= _playersInGame.Count / 2.0)
            {
                IsSomeBodyDied = true;
                KillPlayer(deadP);
            }

            foreach (var player in _playersInGame)
                player.ResetVoteCount();
            Status = Status.MafiaKilling;
            CheckWin();
        }

        private void EndNight()
        {
            _deadPlayers.Clear();
            foreach (var player in _allPlayers.Where(player => player.Role is MafiaRole))
                player.Role.act = false;
            var deadP = _playersInGame.OrderByDescending(x => x.KillCount).First();
            foreach (var player in _playersInGame)
                player.ResetKillCount();
            IsSomeBodyDied = true;
            KillPlayer(deadP);
            Status = Status.Voting;
            CheckWin();
        }
        
        public OperationStatus Vote(string voter, string target)
        {
            if (_playersInGame.All(x => x.Name != voter))
                return OperationStatus.NotInGame;
            if (_votedPlayers.Contains(voter))
                return OperationStatus.Already;
            var targetP = _playersInGame.FirstOrDefault(x => x.Name == target);
            if (targetP is null)
                return OperationStatus.Cant;
            _votedPlayers.Add(voter);
            targetP.VoteMe();
            if (_playersInGame.Sum(player => player.VoteCount) == _playersInGame.Count)
                EndDay();
            return OperationStatus.Success;
        }

        public OperationStatus Act(Player maker, int target)
        {
            if (_playersInGame.All(x => x.Name != maker.Name))
                return OperationStatus.NotInGame;
            if (target > _playersInGame.Count || target <= 0)
                return OperationStatus.Incorrect;
            var actStatus = maker.Role.Act(target, _playersInGame);
            switch (actStatus)
            {
                case ActStatus.Already:
                    return OperationStatus.Already;
                case ActStatus.WrongAct:
                    return OperationStatus.WrongAct;
                case ActStatus.EndNight:
                    EndNight();
                    break;
            }

            return OperationStatus.Success;
        }

        private void KillPlayer(Player deadP)
        {
            _deadPlayers.Add(deadP);
            _playersInGame.Remove(deadP);
            _mafiozyPlayers.Remove(deadP);
        }

        private void CheckWin()
        {
            if (_mafiozyPlayers.Count == 0)
                Status = Status.PeacefulWins;
            else if (_mafiozyPlayers.Count >= _playersInGame.Count / 2.0)
                Status = Status.MafiaWins;
        }

        public IReadOnlyCollection<string> GetWinners()
        {
            if (Status == Status.PeacefulWins)
            {
                return _allPlayers
                    .Where(p => p.Role is PeacefulRole)
                    .Select(p => p.Name).ToArray();
            }
            if (Status == Status.MafiaWins)
            {
                return _allPlayers
                    .Where(p => p.Role is MafiaRole)
                    .Select(p => p.Name).ToArray();
            }

            return Array.Empty<string>();
        }

        public IReadOnlyCollection<Player> AllPlayers => _allPlayers;
        public IReadOnlyCollection<string> PlayersInGame => _playersInGame.Select(p => p.Name).ToArray();
        public IReadOnlyDictionary<string, Role> PlayersRoles => _allPlayers.ToDictionary(p => p.Name, p => p.Role);
        public IReadOnlyCollection<string> MafiozyPlayers => _mafiozyPlayers.Select(p => p.Name).ToArray();
        public IReadOnlyCollection<string> Dead => _deadPlayers.Select(p => p.Name).ToArray();
        public IReadOnlyDictionary<int, string> PlayersNumbers => _playersNumbers;
        public IReadOnlyCollection<string> DeadPlayers => _deadPlayers.Select(p => p.Name).ToArray();
    }
}
