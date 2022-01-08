using System.Collections.Generic;
using System.Linq;

namespace Mafia
{
    public abstract class Role
    {
        public bool act;
        public abstract string GetDescription();
        public abstract ActStatus Act(Player target, out PlayerState state);
    }

    public class PeacefulRole : Role
    {
        public override string GetDescription() => 
            "You are Peaceful. You can nothing, just try to survive.";

        public override ActStatus Act(Player target, out PlayerState state)
        {
            state = PlayerState.Alive;
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

        public override ActStatus Act(Player target, out PlayerState state)
        {
            if (act)
            {
                state = PlayerState.Dead;
                return ActStatus.Already;
            }

            act = true;
            target.KillMe();
            state = PlayerState.Dead;
            return ActStatus.Success;
            // playersInGame.Sum(player => player.KillCount) ==
            //    playersInGame.Sum(player => player.Role is MafiaRole ? 1 : 0) 
            // ? ActStatus.EndNight : ActStatus.Nothing;
        }

        public override string ToString()
        {
            return "Мафия";
        }
    }

    public class DoctorRole : Role
    {
        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
        

        public override ActStatus Act(Player target, out PlayerState state)
        {
            if (act)
            {
                state = PlayerState.Alive;
                return ActStatus.Already;
            }

            act = true;
            target.HealMe();
            state = PlayerState.Alive;
            return ActStatus.Success;
        }
    }
}
