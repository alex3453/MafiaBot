﻿using System.Collections.Generic;

namespace Mafia
{
    public interface IMafia
    {
        Status Status { get; }
        bool IsSomeBodyDied { get; }
        IReadOnlyCollection<Player> AllPlayers { get; }
        IReadOnlyCollection<string> PlayersInGame { get; }
        IReadOnlyCollection<string> MafiozyPlayers { get; }
        IReadOnlyDictionary<string, Role> PlayersRoles { get; }
        IReadOnlyDictionary<int, string> PlayersNumbers { get; }
        OperationStatus RegisterPlayer(string name);
        void StartGame();
        IReadOnlyCollection<string> DeadPlayers { get; }
        OperationStatus Vote(string voter, string target);
        OperationStatus Act(Player maker, int target);
        IReadOnlyCollection<string> GetWinners();
    }
}