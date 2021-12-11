using System;
using System.Collections.Generic;
using System.Linq;
using Mafia;
using CommonInteraction;

namespace App
{
    public class Bot
    {
        private readonly IDictionary<ulong, UsersTeam> usersTeams = new Dictionary<ulong, UsersTeam>();

        public Action<User, bool, Command> Register() => ReproduceCommand;
        public event Action<User, bool, Answer> SendMassage;
        public event Action<ulong> DeleteUser;

        private void CreateNewUsersTeam(User user)
        {
            usersTeams[user.ComChatId] = new UsersTeam();
        }

        private void ReproduceCommand (User user, bool isCommonChat, Command ctx)
        {
            if (!usersTeams.Keys.Contains(user.ComChatId))
                CreateNewUsersTeam(user);
            if (!usersTeams[user.ComChatId].IsContainsUser(user))
                usersTeams[user.ComChatId].AddUser(user);

            switch (ctx.CommandType)
            {
                case CommandType.CreateNewGame:
                    CreateNewGame(user, isCommonChat);
                    break;
                case CommandType.Reg:
                    RegPlayer(user, isCommonChat);
                    break;
                case CommandType.Start:
                    StartGame(user, isCommonChat);
                    break;
                case CommandType.Vote:
                    Vote(user, isCommonChat, ctx.MentionedPlayers);
                    break;
                case CommandType.Kill:
                    KillPlayer(user, isCommonChat, ctx.MentionedPlayers);
                    break;
            }
        }
        
        private void CreateNewGame(User user, bool isCommonChat)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }
            usersTeams[user.ComChatId].DeleteAllUsers(DeleteUser);
            usersTeams[user.ComChatId].SetMafia();
            SendMassage?.Invoke(user, true, new Answer(AnswerType.NewGame));
        }
        
        private void RegPlayer(User user, bool isCommonChat)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }
            var mafia = usersTeams[user.ComChatId].Mafia;
            if (mafia.Status != Status.WaitingPlayers)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.GameIsGoing));
                return;
            }
            if (mafia.AllPlayers.Contains(user.Name))
            {
                SendMassage?.Invoke(user, true,
                    new Answer(AnswerType.AlreadyRegistered, new List<string> {user.Name}));
            }
            else
            {
                mafia.RegisterPlayer(user.Name);
                SendMassage?.Invoke(user, true,
                    new Answer(AnswerType.SuccessfullyRegistered, new List<string> {user.Name}));
            }
        }
        
        private void StartGame(User user, bool isCommonChat)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }
            var userTeam = usersTeams[user.ComChatId];
            var mafia = usersTeams[user.ComChatId].Mafia;
            if (mafia.Status != Status.ReadyToStart)
            {
                if (mafia.Status == Status.WaitingPlayers)
                {
                    SendMassage?.Invoke(user, true, new Answer(AnswerType.NeedMorePlayers));
                    return;
                }
                SendMassage?.Invoke(user, true, new Answer(AnswerType.GameIsGoing));
                return;
            }
            mafia.StartGame();
            var playersRoles = mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = userTeam.Users.First(u => u.Name == player);
                SendMassage?.Invoke(usr, false, new Answer(GetRoleAnswerType(playersRoles[player])));
            }
            SendMassage?.Invoke(user, true, new Answer(AnswerType.GameStarted));
        }
        
        private AnswerType GetRoleAnswerType(Role role)
        {
            if (role.GetType() == typeof(PeacefulRole))
                return AnswerType.YouArePeaceful;
            if (role.GetType() == typeof(MafiaRole))
                return AnswerType.YouAreMafia;
            throw new ArgumentException("Неопознанная роль");
        }

        private void Vote(User user, bool isCommonChat, IEnumerable<string> mentionedPlayers)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }
            var target = mentionedPlayers.First();
            var mafia = usersTeams[user.ComChatId].Mafia;
            var target = mafia.GetAllPlayers.First(x => x.Name == targetName);
            var voter = mafia.GetAllPlayers.First(x => x.Name == user.Name);
            var res = mafia.Vote(voter, target);
            Answer answ;
            switch (mafia.Status)
            {
                case Status.MafiaWins:
                    answ = new Answer(AnswerType.MafiaWins, new List<string> {mafia.Dead.Name});
                    break;
                case Status.PeacefulWins:
                    answ = new Answer(AnswerType.PeacefulWins, new List<string> {mafia.Dead.Name});
                    break;
                default:
                    answ = new Answer(res ? AnswerType.SuccessfullyVoted : AnswerType.UnsuccessfullyVoted);
                        break;
            }
            SendMassage?.Invoke(user, true, answ);
        }

        private void KillPlayer(User user, bool isCommonChat, IEnumerable<string> mentionedPlayers)
        {
            if (mafia.Status != Status.Night)
                return new Answer(true, AnswerType.DayKill);
        
            var target = mafia.GetAllPlayers.First(x => x.Name == victimName);
            var killer = mafia.GetAllPlayers.First(x => x.Name == killerName);
            return killer.Role is MafiaRole 
                ? mafia.Kill(killer, target)
                : new Answer(true, AnswerType.NotMafia);
        }
    }
}