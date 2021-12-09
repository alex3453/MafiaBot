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
        Died,
        NewGame,
        YouAreMafia,
        YouArePeaceful
    }
}