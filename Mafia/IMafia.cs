using System.Collections.Generic;

namespace Mafia
{
    public interface IMafia
    {
        Status Status { get; }
        int NightNumber { get; }
        IReadOnlyList<Player> GetAllPlayers { get; }
        void RegisterPlayer(Player player);
        void StartGame();
        void EndDay();
        void EndNight();
        List<Player> Winners { get; }
        Player Dead { get; }
        bool Vote(Player voter, Player target);
        //bool Kill(Player killer, Player target);
        IReadOnlyList<Role> GetRoles { get; }
    };
    }