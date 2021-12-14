using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class SimpleTypesHandler : IAnswerTypeHandler
    {
        public bool IsItMyAnswerType(AnswerType answerType)
        {
            return answerType is
                AnswerType.AlreadyKilled or
                AnswerType.AlreadyRegistered or
                AnswerType.AlreadyVoted or
                AnswerType.EndDay or
                AnswerType.EndNight or
                AnswerType.EnterNumber or
                AnswerType.IncorrectNumber or
                AnswerType.IncorrectVote or
                AnswerType.NewGame or
                AnswerType.SuccessfullyKilled or
                AnswerType.SuccessfullyRegistered or
                AnswerType.SuccessfullyVoted or
                AnswerType.DayAllAlive or
                AnswerType.GameIsGoing or
                AnswerType.NeedMorePlayers or
                AnswerType.NightAllAlive or
                AnswerType.OnlyInCommon or
                AnswerType.OnlyInLocal or
                AnswerType.NotTimeToKill or
                AnswerType.NotTimeToVote or
                AnswerType.YouAreNotInGame or
                AnswerType.YouCantKillThisPl or
                AnswerType.YouCantVoteThisPl or
                AnswerType.PeacefulWins or
                AnswerType.MafiaWins;
        }

        public void SendMessage(
            AnswerType answerType, 
            UsersTeam usersTeam, 
            CommandInfo commandInfo,
            Action<User, bool, Answer, ulong> send)
        {
            var args = new List<string>();
            args.Add(commandInfo.User.Name);
            if (commandInfo.Content.Any())
                args.Add(commandInfo.Content.First());
            else if (commandInfo.MentionedPlayers.Any())
                args.Add(commandInfo.MentionedPlayers.First());
            var id = commandInfo.IsCommonChannel ? usersTeam.ChatId : commandInfo.User.Id;
            send(commandInfo.User, commandInfo.IsCommonChannel, new Answer(answerType, args), id);
        }
    }

    public class StartGameTypeHandler : IAnswerTypeHandler
    {
        public bool IsItMyAnswerType(AnswerType answerType)
        {
            return answerType == AnswerType.GameStarted;
        }

        public void SendMessage(AnswerType answerType, UsersTeam usersTeam, CommandInfo commandInfo, Action<User, bool, Answer, ulong> send)
        {
            
            var playersRoles = usersTeam.Mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = usersTeam.Users.First(u => u.Name == player);
                send(usr, false, new Answer(GetRoleAnswerType(playersRoles[player])), usr.CommonChannelId);
            }

            send(commandInfo.User, true, new Answer(AnswerType.GameStarted), usersTeam.ChatId);
        }
        
        private AnswerType GetRoleAnswerType(Role role)
        {
            if (role.GetType() == typeof(PeacefulRole))
                return AnswerType.YouArePeaceful;
            if (role.GetType() == typeof(MafiaRole))
                return AnswerType.YouAreMafia;
            throw new ArgumentException("Неопознанная роль");
        }
    }

    public class StartMafiaKillingTypeHandler : IAnswerTypeHandler
    {
        public bool IsItMyAnswerType(AnswerType answerType)
        {
            return answerType == AnswerType.MafiaKilling;
        }

        public void SendMessage(AnswerType answerType, UsersTeam usersTeam, CommandInfo commandInfo, Action<User, bool, Answer, ulong> send)
        {
            var mafia = usersTeam.Mafia;
            var mafiaKillList = new List<string>();
            foreach (var playerNumber in mafia.PlayersNumbers)
            {
                if (mafia.PlayersInGame.Contains(playerNumber.Value))
                {
                    mafiaKillList.Add(playerNumber.Key.ToString());
                    mafiaKillList.Add(playerNumber.Value);
                }
            }
            foreach (var player in mafia.MafiozyPlayers)
            {
                var usr = usersTeam.Users.First(u => u.Name == player);
                send(usr, false, new Answer(AnswerType.MafiaKilling, mafiaKillList), usr.Id);
            }
        }
    }
}