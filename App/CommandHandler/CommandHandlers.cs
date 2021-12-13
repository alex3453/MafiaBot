using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class ResetGameCommand : ICommandHandler
    {
        public AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
                return AnswerType.OnlyInCommon;
            usersTeam.ResetMafia();
            return AnswerType.NewGame;
        }
    }
    
    public class RegPlayerCommand : ICommandHandler
    {
        public AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (usersTeam.ContainsUser(commandInfo.User))
                return AnswerType.AlreadyRegistered;
            var status = usersTeam.Mafia.Status;
            if (status is not (Status.WaitingPlayers or Status.WaitingPlayers))
                return AnswerType.GameIsGoing;
            usersTeam.AddUser(commandInfo.User);
            usersTeam.Mafia.RegisterPlayer(commandInfo.User.Name);
            return AnswerType.SuccessfullyRegistered;
        }
    }

    public class StartCommand : ICommandHandler
    {
        public AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
                return AnswerType.OnlyInCommon;
            var status = usersTeam.Mafia.Status;
            if (status != Status.ReadyToStart) 
                return status == Status.WaitingPlayers ? AnswerType.NeedMorePlayers : AnswerType.GameIsGoing;
            usersTeam.Mafia.StartGame();
            return AnswerType.NewGame;
        }
    }
    
    public class VoteCommand : ICommandHandler
    {
        public AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
                return AnswerType.OnlyInCommon;
            var status = usersTeam.Mafia.Status;
            if (status != Status.Voting)
                return AnswerType.NotTimeToVote;
            if (!commandInfo.MentionedPlayers.Any())
                return AnswerType.IncorrectVote;
            if (!usersTeam.ContainsUser(commandInfo.User))
                return AnswerType.YouAreNotInGame;
            var user = commandInfo.User.Name;
            var target = commandInfo.MentionedPlayers.First();
            var opStatus = usersTeam.Mafia.Vote(user, target);
            return opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public class KillCommand : ICommandHandler
    {
        public AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (commandInfo.IsCommonChannel)
                return AnswerType.OnlyInLocal;
            var status = usersTeam.Mafia.Status;
            if (status != Status.MafiaKilling)
                return AnswerType.NotTimeToKill;
            if (!usersTeam.ContainsUser(commandInfo.User))
                return AnswerType.YouAreNotInGame;
            var user = commandInfo.User.Name;
            int target;
            if (!commandInfo.Content.Any() || !int.TryParse(commandInfo.Content.First(), out target))
                return AnswerType.IncorrectNumber;
            var opStatus = usersTeam.Mafia.Vote()
        }
    }
}