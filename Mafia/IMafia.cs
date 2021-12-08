using System.Collections.Generic;
using CommonInteraction;

namespace Mafia
{
    public interface IMafia
    {
        Status Status { get; }
        int NightNumber { get; }
        IReadOnlyList<Player> GetAllPlayers { get; }
        void RegisterPlayer(Player player);
        Answer StartGame();
        Answer EndDay();
        Answer EndNight();
        List<Player> Winners { get; }
        Answer Vote(Player voter, Player target);
        Answer Kill(Player killer, Player target);
        Answer GetRules();
        IReadOnlyList<Role> GetRoles { get; }
    };
    }