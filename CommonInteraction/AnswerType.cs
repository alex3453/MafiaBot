namespace CommonInteraction
{
    public enum AnswerType
    {
        GameStarted,
        GetRules,
        MafiaWins,
        PeacefulWins,
        SuccessfullyRegistered,
        AlreadyRegistered,
        SuccessfullyVoted,
        AlreadyVoted,
        EndDay,
        EndNight,
        DayKill,
        DayAllAlive,
        NightKill,
        NightAllAlive,
        NewGame,
        YouAreMafia,
        YouArePeaceful,
        OnlyInLocal,
        OnlyInCommon,
        GameIsGoing,
        NeedMorePlayers,
        YouAreNotInGame,
        YouCantVoteThisPl,
        NotTimeToVote,
        NotTimeToKill,
        EnterNumber
    }
}