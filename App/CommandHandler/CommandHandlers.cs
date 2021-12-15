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
                return;
            }
            if (gT.ContainsUser(cI.User))
            {
                send(true, new Answer(AnswerType.AlreadyRegistered, cI.User.Name), cI.ComChatId);
                return;
            }
            if (gT.Mafia.Status is not (Status.WaitingPlayers or Status.ReadyToStart))
            {
                send(true, new Answer(AnswerType.GameIsGoing, cI.User.Name), cI.ComChatId);
                return;
            }
            gT.AddUser(cI.User);
            send(true, new Answer(AnswerType.SuccessfullyRegistered, cI.User.Name), cI.ComChatId);
        }
    }

    public class StartCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Start;
        
        public void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send)
        {
            if (!cI.IsComChat)
            {
                send(false, new Answer(AnswerType.OnlyInCommon, cI.User.Name), cI.User.Id);
                return;
            }

            if (gT.Mafia.Status is not Status.ReadyToStart)
            {
                send(true,
                    gT.Mafia.Status is Status.WaitingPlayers
                        ? new Answer(AnswerType.NeedMorePlayers, gT.Users.Select(u => u.Name).ToArray())
                        : new Answer(AnswerType.GameIsGoing),
                    cI.ComChatId);
                return;
            }
            gT.Mafia.StartGame();
            var playersRoles = gT.Mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = gT.Users.First(u => u.Name == player);
                send(false, new Answer(AnswerType.TellRole, playersRoles[usr.Name].ToString()), usr.Id);
            }
            send(true, new Answer(AnswerType.GameStarted, gT.Users.Select(u => u.Name).ToArray()), gT.ChatId);
        }
    }

    public class VoteCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Vote;
        
        public void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send)
        {
            if (!cI.IsComChat)
            {
                send(false, new Answer(AnswerType.OnlyInCommon, cI.User.Name), cI.User.Id);
                return;
            }

            if (!gT.ContainsUser(cI.User))
            {
                send(true, new Answer(AnswerType.YouAreNotInGame, cI.User.Name), cI.ComChatId);
                return;
            }
            
            if (gT.Mafia.Status is not Status.Voting)
            {
                send(true, new Answer(AnswerType.NotTimeToVote, cI.User.Name), cI.ComChatId);
                return;
            }

            if (!cI.MentPlayers.Any())
            {
                send(true, new Answer(AnswerType.IncorrectVote, cI.User.Name), cI.ComChatId);
                return;
            }

            var voter = cI.User.Name;
            var target = cI.MentPlayers.First();
            var opStatus = gT.Mafia.Vote(voter, target);
            var answType = opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            send(true, new Answer(answType, cI.User.Name, target), gT.ChatId);
            

            if (gT.Mafia.Status is Status.MafiaKilling)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    send(true, new Answer(AnswerType.DayKill, gT.Mafia.Dead.ToArray()), gT.ChatId);
                else
                    send(true, new Answer(AnswerType.DayAllAlive), gT.ChatId);
                send(true, new Answer(AnswerType.EndDay), gT.ChatId);
                var killList = gT.Mafia.PlayersInGame
                    .Select((s, i) => (s, (i + 1).ToString()))
                    .SelectMany(t => new[] {t.s, t.Item2})
                    .ToArray();
                foreach (var player in gT.Mafia.MafiozyPlayers)
                {
                    var usr = gT.Users.First(u => u.Name == player);
                    send(false, new Answer(AnswerType.MafiaKilling, killList), usr.Id);
                }

                return;
            }
            if (gT.Mafia.Status is Status.MafiaWins)
            {
                send(true, new Answer(AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
                return;
            }

            if (gT.Mafia.Status is Status.PeacefulWins)
            {
                send(true, new Answer(AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
                return;
            }
        }
    }

    public class KillCommand : ICommandHandler
    {
        public CommandType Type => CommandType.Kill;
        
        public void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send)
        {
            if (gT is null)
            {
                send(false, new Answer(AnswerType.YouAreNotInGame, cI.User.Name), cI.User.Id);
                return;
            }

            if (cI.IsComChat)
            {
                send(true, new Answer(AnswerType.OnlyInLocal, cI.User.Name), cI.ComChatId);
                return;
            }
                
            if (!gT.ContainsUser(cI.User))
            {
                send(false, new Answer(AnswerType.YouAreNotInGame, cI.User.Name), cI.User.Id);
                return;
            }

            if (gT.Mafia.Status is not Status.MafiaKilling)
            {
                send(false, new Answer(AnswerType.NotTimeToKill, cI.User.Name), cI.User.Id);
                return;
            }

            if (!cI.Content.Any() || !int.TryParse(cI.Content.First(), out var target))
            {
                send(false, new Answer(AnswerType.IncorrectNumber, cI.User.Name), cI.User.Id);
                return;
            }
            var killer = cI.User.Name;
            var opStatus = gT.Mafia.Act(gT.Mafia.AllPlayers.Single(player => player.Name == killer), target);
            var answType =  opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            send(false, new Answer(answType, cI.User.Name, target.ToString()), cI.User.Id);

            if (gT.Mafia.Status is Status.Voting)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    send(true, new Answer(AnswerType.NightKill, gT.Mafia.Dead.ToArray()), gT.ChatId);
                else
                    send(true, new Answer(AnswerType.NightAllAlive), gT.ChatId);

                send(true, new Answer(AnswerType.EndNight), gT.ChatId);
            }
            else if (gT.Mafia.Status is Status.MafiaWins)
                send(true, new Answer(AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
            else if (gT.Mafia.Status is Status.PeacefulWins)
                send(true, new Answer(AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
        }
    }
}