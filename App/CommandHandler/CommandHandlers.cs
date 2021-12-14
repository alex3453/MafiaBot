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
        public void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send)
        {
            if (!cI.IsComChat)
                send(false, new Answer(AnswerType.OnlyInCommon), cI.User.Id);
            else
            {
                gT.Reset();
                send(true, new Answer(AnswerType.NewGame), cI.ComChatId);
            }
        }
    }

    public class RegPlayerCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Reg;
        public void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send)
        {
            if (!cI.IsComChat)
            {
                send(false, new Answer(AnswerType.OnlyInCommon, cI.User.Name), cI.User.Id);
            }
            else if (gT.ContainsUser(cI.User))
            {
                send(true, new Answer(AnswerType.AlreadyRegistered, cI.User.Name), cI.ComChatId);
            }
            else if (gT.Mafia.Status is not (Status.WaitingPlayers or Status.ReadyToStart))
            {
                send(true, new Answer(AnswerType.GameIsGoing, cI.User.Name), cI.ComChatId);
            }
            else
            {
                gT.AddUser(cI.User);
                send(true, new Answer(AnswerType.SuccessfullyRegistered, cI.User.Name), cI.ComChatId);
            }
        }

        public IEnumerable<AnswerType> ExecuteCommand(GameTeam gameTeam, CommandInfo commandInfo)
        {
            if (gameTeam.ContainsUser(commandInfo.User))
            {
                yield return AnswerType.AlreadyRegistered;
                yield break;
            }

            var status = gameTeam.Mafia.Status;
            if (status is not (Status.WaitingPlayers or Status.WaitingPlayers))
            {
                yield return AnswerType.GameIsGoing;
                yield break;
            }

            gameTeam.AddUser(commandInfo.User);
            gameTeam.Mafia.RegisterPlayer(commandInfo.User.Name);
            yield return AnswerType.SuccessfullyRegistered;
        }
    }

    public class StartCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Start;

        public IEnumerable<AnswerType> ExecuteCommand(GameTeam gameTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsComChat)
            {
                yield return AnswerType.OnlyInCommon;
                yield break;
            }

            var status = gameTeam.Mafia.Status;
            if (status != Status.ReadyToStart)
            {
                yield return status == Status.WaitingPlayers ? AnswerType.NeedMorePlayers : AnswerType.GameIsGoing;
                yield break;
            }

            gameTeam.Mafia.StartGame();
            yield return AnswerType.GameStarted;
        }
    }

    public class VoteCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Vote;

        public IEnumerable<AnswerType> ExecuteCommand(GameTeam gameTeam, CommandInfo commandInfo)
        {
            if (!commandInfo.IsComChat)
            {
                yield return AnswerType.OnlyInCommon;
                yield break;
            }

            var status = gameTeam.Mafia.Status;
            if (status != Status.Voting)
            {
                yield return AnswerType.NotTimeToVote;
                yield break;
            }

            if (!commandInfo.MentPlayers.Any())
            {
                yield return AnswerType.IncorrectVote;
                yield break;
            }

            if (!gameTeam.ContainsUser(commandInfo.User))
            {
                yield return AnswerType.YouAreNotInGame;
                yield break;
            }

            var voter = commandInfo.User.Name;
            var target = commandInfo.MentPlayers.First();
            var opStatus = gameTeam.Mafia.Vote(voter, target);
            yield return opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var answerType in CheckStatusChange(gameTeam)) yield return answerType;
        }

        private IEnumerable<AnswerType> CheckStatusChange(GameTeam gameTeam)
        {
            if (gameTeam.Mafia.Status == Status.MafiaKilling)
            {
                if (gameTeam.Mafia.IsSomeBodyDied)
                    yield return AnswerType.DayKill;
                else
                {
                    yield return AnswerType.DayAllAlive;
                }

                yield return AnswerType.EndDay;
                yield return AnswerType.MafiaKilling;
            }

            if (gameTeam.Mafia.Status == Status.PeacefulWins)
                yield return AnswerType.PeacefulWins;
            if (gameTeam.Mafia.Status == Status.MafiaWins)
                yield return AnswerType.MafiaWins;
        }
    }

    public class KillCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Kill;

        public IEnumerable<AnswerType> ExecuteCommand(GameTeam gameTeam, CommandInfo commandInfo)
        {
            if (commandInfo.IsComChat)
            {
                yield return AnswerType.OnlyInLocal;
                yield break;
            }

            var status = gameTeam.Mafia.Status;
            if (status != Status.MafiaKilling)
            {
                yield return AnswerType.NotTimeToKill;
                yield break;
            }

            if (!gameTeam.ContainsUser(commandInfo.User))
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

            var opStatus = gameTeam.Mafia.Act(killer, target);
            yield return opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var answerType in CheckStatusChange(gameTeam)) yield return answerType;
        }

        private IEnumerable<AnswerType> CheckStatusChange(GameTeam gameTeam)
        {
            if (gameTeam.Mafia.Status == Status.Voting)
            {
                if (gameTeam.Mafia.IsSomeBodyDied)
                    yield return AnswerType.NightKill;
                else
                    yield return AnswerType.NightAllAlive;
                yield return AnswerType.EndNight;
            }

            if (gameTeam.Mafia.Status == Status.PeacefulWins)
                yield return AnswerType.PeacefulWins;
            if (gameTeam.Mafia.Status == Status.MafiaWins)
                yield return AnswerType.MafiaWins;
        }
    }
}