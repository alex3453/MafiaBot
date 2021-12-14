using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class ResetGameCommand : ICommandHandler
    {
        public CommandType Type => CommandType.CreateNewGame;

        public IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
            {
                yield return AnswerType.OnlyInCommon;
                yield break;
            }

            usersTeam.ResetMafia();
            yield return AnswerType.NewGame;
        }
    }

    public class RegPlayerCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Reg;

        public IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (usersTeam.ContainsUser(commandInfo.User))
            {
                yield return AnswerType.AlreadyRegistered;
                yield break;
            }

            var status = usersTeam.Mafia.Status;
            if (status is not (Status.WaitingPlayers or Status.WaitingPlayers))
            {
                yield return AnswerType.GameIsGoing;
                yield break;
            }

            usersTeam.AddUser(commandInfo.User);
            usersTeam.Mafia.RegisterPlayer(commandInfo.User.Name);
            yield return AnswerType.SuccessfullyRegistered;
        }
    }

    public class StartCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Start;

        public IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
            {
                yield return AnswerType.OnlyInCommon;
                yield break;
            }

            var status = usersTeam.Mafia.Status;
            if (status != Status.ReadyToStart)
            {
                yield return status == Status.WaitingPlayers ? AnswerType.NeedMorePlayers : AnswerType.GameIsGoing;
                yield break;
            }

            usersTeam.Mafia.StartGame();
            yield return AnswerType.GameStarted;
        }
    }

    public class VoteCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Vote;

        public IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsCommonChannel)
            {
                yield return AnswerType.OnlyInCommon;
                yield break;
            }

            var status = usersTeam.Mafia.Status;
            if (status != Status.Voting)
            {
                yield return AnswerType.NotTimeToVote;
                yield break;
            }

            if (!commandInfo.MentionedPlayers.Any())
            {
                yield return AnswerType.IncorrectVote;
                yield break;
            }

            if (!usersTeam.ContainsUser(commandInfo.User))
            {
                yield return AnswerType.YouAreNotInGame;
                yield break;
            }

            var voter = commandInfo.User.Name;
            var target = commandInfo.MentionedPlayers.First();
            var opStatus = usersTeam.Mafia.Vote(voter, target);
            yield return opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var answerType in CheckStatusChange(usersTeam)) yield return answerType;
        }

        private IEnumerable<AnswerType> CheckStatusChange(UsersTeam usersTeam)
        {
            if (usersTeam.Mafia.Status == Status.MafiaKilling)
            {
                if (usersTeam.Mafia.IsSomeBodyDied)
                    yield return AnswerType.DayKill;
                else
                {
                    yield return AnswerType.DayAllAlive;
                }

                yield return AnswerType.EndDay;
                yield return AnswerType.MafiaKilling;
            }

            if (usersTeam.Mafia.Status == Status.PeacefulWins)
                yield return AnswerType.PeacefulWins;
            if (usersTeam.Mafia.Status == Status.MafiaWins)
                yield return AnswerType.MafiaWins;
        }
    }

    public class KillCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Kill;

        public IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo)
        {
            if (commandInfo.IsCommonChannel)
            {
                yield return AnswerType.OnlyInLocal;
                yield break;
            }

            var status = usersTeam.Mafia.Status;
            if (status != Status.MafiaKilling)
            {
                yield return AnswerType.NotTimeToKill;
                yield break;
            }

            if (!usersTeam.ContainsUser(commandInfo.User))
            {
                yield return AnswerType.YouAreNotInGame;
                yield break;
            }

            var killer = commandInfo.User.Name;
            int target;
            if (!commandInfo.Content.Any() || !int.TryParse(commandInfo.Content.First(), out target))
            {
                yield return AnswerType.IncorrectNumber;
                yield break;
            }

            var opStatus = usersTeam.Mafia.Act(killer, target);
            yield return opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var answerType in CheckStatusChange(usersTeam)) yield return answerType;
        }

        private IEnumerable<AnswerType> CheckStatusChange(UsersTeam usersTeam)
        {
            if (usersTeam.Mafia.Status == Status.Voting)
            {
                if (usersTeam.Mafia.IsSomeBodyDied)
                    yield return AnswerType.NightKill;
                else
                {
                    yield return AnswerType.NightAllAlive;
                }

                yield return AnswerType.EndNight;
            }

            if (usersTeam.Mafia.Status == Status.PeacefulWins)
                yield return AnswerType.PeacefulWins;
            if (usersTeam.Mafia.Status == Status.MafiaWins)
                yield return AnswerType.MafiaWins;
        }
    }
}