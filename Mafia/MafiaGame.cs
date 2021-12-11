﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Mafia
{
    public class MafiaGame : IMafia
    {
        private readonly List<Player> allPlayers = new();
        private readonly List<Player> playersInGame = new();
        private readonly List<Player> deadPlayers = new();
        private readonly List<Player> mafiozyPlayers = new();
        private HashSet<string> votedPlayers = new();
        private Dictionary<string, int> playersNumbers = new();
        private IRoleDistribution roleDist;
        public string Dead { get; private set; }
        public bool IsSomeBodyDied { get; private set; }

        public Status Status { get; private set; } = Status.WaitingPlayers;

        public MafiaGame(IRoleDistribution roleDist)
        {
            this.roleDist = roleDist;
        }

        public void RegisterPlayer(string name)
        {
            var player = new Player(name);
            allPlayers.Add(player);
            if (allPlayers.Count >= 2)
                Status = Status.ReadyToStart;
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
                playersNumbers[allPlayers[i].Name] = i + 1;
            }
            Status = Status.MafiaKilling;
        }
        

        private void EndDay()
        {
            IsSomeBodyDied = false;
            votedPlayers = new HashSet<string>();
            var deadP = playersInGame.OrderByDescending(x => x.VoteCount).First();
            if (deadP.VoteCount >= playersInGame.Count / 2)
            {
                IsSomeBodyDied = true;
                KillPlayer(deadP);
            }
            Status = Status.MafiaKilling;
            CheckWin();
        }

        private void EndNight()
        {
            var deadP = playersInGame.OrderByDescending(x => x.KillCount).First();
            IsSomeBodyDied = true;
            KillPlayer(deadP);
            Status = Status.Voting;
            CheckWin();
        }
        
        public bool Vote(string voter, string target)
        {
            if (votedPlayers.Contains(voter))
                return false;
            var voterP = playersInGame.First(x => x.Name == voter);
            var targetP = playersInGame.First(x => x.Name == target);
            votedPlayers.Add(voter);
            targetP.VoteMe();
            if (playersInGame.Sum(player => player.VoteCount) == playersInGame.Count)
                EndDay();
            return true;
        }

        public bool Act(string killer, string target)
        {
            var killerP = playersInGame.First(x => x.Name == killer);
            var targetP = playersInGame.First(x => x.Name == target);
            if (killerP.Role is MafiaRole)
                return false;
            targetP.KillMe();
            if (playersInGame.Sum(player => player.KillCount) == mafiozyPlayers.Count)
                EndNight();
            return true;
        }

        private void KillPlayer(Player deadP)
        {
            deadPlayers.Add(deadP);
            Dead = deadP.Name;
            playersInGame.Remove(deadP);
            mafiozyPlayers.Remove(deadP);
        }

        private void CheckWin()
        {
            if (mafiozyPlayers.Count == 0)
                Status = Status.PeacefulWins;
            else if (mafiozyPlayers.Count >= playersInGame.Count / 2)
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

        public IReadOnlyCollection<string> AllPlayers => allPlayers.Select(p => p.Name).ToArray();
        public IReadOnlyCollection<string> PlayersInGame => playersInGame.Select(p => p.Name).ToArray();
        public IReadOnlyDictionary<string, Role> PlayersRoles => allPlayers.ToDictionary(p => p.Name, p => p.Role)
        public IReadOnlyDictionary<string, int> PlayersNumbers => playersNumbers;
        public IReadOnlyCollection<string> DeadPlayers => deadPlayers.Select(p => p.Name).ToArray();
    }
}
