using System.Collections.Generic;
using System.Linq;

namespace Mafia
{
    public abstract class Role
    {
        public bool act;
        public abstract string GetDescription();
        public abstract ActStatus Act(int target, List<Player> playersInGame);
    }

    public class PeacefulRole : Role
    {
        public override string GetDescription() => 
            "You are Peaceful. You can nothing, just try to survive.";

        public override ActStatus Act(int target, List<Player> playersInGame)
        {
            return ActStatus.WrongAct;
        }

        public override string ToString()
        {
            return "Мирный";
        }
    }

    public class MafiaRole : Role
    {
        public override string GetDescription() => 
            "You are Mafia. You and yours mafia-friends can kill one player per night.";

        public override ActStatus Act(int target, List<Player> playersInGame)
        {
            if (act)
                return ActStatus.Already;
            act = true;
            var targetP = playersInGame[target - 1];
            targetP.KillMe();
            return playersInGame.Sum(player => player.KillCount) ==
                   playersInGame.Sum(player => player.Role is MafiaRole ? 1 : 0) 
                ? ActStatus.EndNight : ActStatus.Nothing;
        }

        public override string ToString()
        {
            return "Мафия";
        }
        
        public bool Kill(Player target) => true;
    }
}
