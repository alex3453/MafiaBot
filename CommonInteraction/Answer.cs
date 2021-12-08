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
        public IReadOnlyList<string> Args { get; }
        public IReadOnlyDictionary<string, string> Dict { get; }

        public Answer(bool needToInteract, AnswerType answerType,
            IReadOnlyList<string> args = null)
        {
            NeedToInteract = needToInteract;
            AnswerType = answerType;
            Args = args;
        }
        
        public Answer(bool needToInteract, AnswerType answerType,
            IReadOnlyDictionary<string, string> dict)
        {
            NeedToInteract = needToInteract;
            AnswerType = answerType;
            Dict = dict;
        }
    }
}