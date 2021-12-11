using System.Collections.Generic;

namespace Mafia
{
    public interface IMafia
    {
        Status Status { get; }
        bool IsSomeBodyDied { get; }
        IReadOnlyCollection<string> AllPlayers { get; }
        IReadOnlyCollection<string> PlayersInGame { get; }
        IReadOnlyCollection<string> MafiozyPlayers { get; }
        IReadOnlyDictionary<string, Role> PlayersRoles { get; }
        IReadOnlyDictionary<string, int> PlayersNumbers { get; }
        void RegisterPlayer(string name);
        void StartGame();
        IReadOnlyCollection<string> Dead { get; }
        bool Vote(string voter, string target);
        bool Act(string killer, string target);
        IReadOnlyCollection<string> GetWinners();
    }
}