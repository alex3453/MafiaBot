namespace DiscordBot.Mafia
{
    public abstract class Role
    {
        public abstract string GetDescription();

    }

    public class PeacefulRole : Role
    {
        public override string GetDescription() => 
            "You are Peaceful. You can nothing, just try to survive.";

        public override string ToString()
        {
            return "Мирный";
        }
    }

    public class MafiaRole : Role
    {
        public override string GetDescription() => 
            "You are Mafia. You and yours mafia-friends can kill one player per night.";

        public override string ToString()
        {
            return "Мафия";
        }
        
        public bool Kill(Player target) => true;
    }
}
