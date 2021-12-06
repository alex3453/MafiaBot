using System.Collections.Generic;

namespace DiscordBot.Mafia
{
    public enum Answers
    {
        NeedMorePlayer,
        GameStart, 
        GetRules, 
        MafiaWins, 
        PeacefulWins,
        SuccessfullyRegistered,
        UnsuccessfullyRegistered,
        SuccessfullyVoted,
        UnsuccessfullyVoted,
        SuccessfullyKill,
        UnsuccessfullyKill,
        UnknownCommand,
        EndDay,
        EndNight,
        None,
        DayKill,
        NotMafia
    }
    public class Answer
    {
        public bool NeedToInteract { get; }
        public Answers Answers { get; }
        public IReadOnlyList<string> Args { get; private set; }
        public bool ToPlayer { get; }
 
        public Answer(bool needToInteract, Answers answer = Answers.None, 
            IReadOnlyList<string> args = null, bool toPlayer = false)
        {
            NeedToInteract = needToInteract;
            Answers = answer;
            Args = args;
            ToPlayer = toPlayer;
        }

        public void SetArgs(List<string> list)
        {
            Args = list;
        }
    }
}