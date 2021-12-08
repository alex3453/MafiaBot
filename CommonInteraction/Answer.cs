using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public enum AnswerType
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
        NotMafia,
        DayKill,
        Died
    }

    public class Answer
    {
        public bool NeedToInteract { get; }
        public AnswerType AnswerType { get; }
        public IReadOnlyList<string> Args { get; private set; }
        public bool ToPlayer { get; }

        public Answer(bool needToInteract, AnswerType answerType = AnswerType.None,
            IReadOnlyList<string> args = null, bool toPlayer = false)
        {
            NeedToInteract = needToInteract;
            AnswerType = answerType;
            Args = args;
            ToPlayer = toPlayer;
        }

        public void SetArgs(List<string> list)
        {
            Args = list;
        }
    }
}