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
        public event Action<User> DeleteUser;

        private void CreateNewUsersTeam(User user)
        {
            usersTeams[user.ComChatId] = new UsersTeam();
            usersTeams[user.ComChatId].AddUser(user);
        }

        private void ReproduceCommand (User user, bool isCommonChat, Command ctx)
        {
            if (!usersTeams.Keys.Contains(user.ComChatId))
                CreateNewUsersTeam(user);
            if (!usersTeams[user.ComChatId].IsContainsUser(user))
                usersTeams[user.ComChatId].AddUser(user);
            
            switch (ctx.CommandType)
            {
                case CommandType.Vote:
                    Vote(user, isCommonChat, ctx.MentionedPlayers);
                    break;
                case CommandType.Rules:
                    SendRules(user, isCommonChat);
                    break;
                case CommandType.Start:
                    StartGame(user, isCommonChat);
                    break;
                case CommandType.Reg:
                    RegPlayer(user, isCommonChat);
                    break;
                case CommandType.Kill:
                    break;
                case CommandType.CreateNewGame:
                    CreateNewGame(user, isCommonChat);
                    break;
                default:
                    SendUnknownCom(user, isCommonChat);
                    break;
            }
        }

        private void SendUnknownCom(User user, bool isCommonChat)
        {
            SendMassage?.Invoke(user, isCommonChat, new Answer(AnswerType.UnknownCommand));
        }

        private void Vote(User user, bool isCommonChat, IEnumerable<string> mentionedPlayers)
        {
            if (!isCommonChat) throw new ArgumentException("Голосовать можно только в чате");
            var targetName = mentionedPlayers.First();
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

        private void SendRules(User user, bool isCommonChat) => 
            SendMassage?.Invoke(user, isCommonChat, new Answer(AnswerType.GetRules));
        
        private void CreateNewGame(User user, bool isCommonChat)
        {
            if (!isCommonChat) throw new ArgumentException("Игру создать можно только в чате");
            usersTeams[user.ComChatId].SetMafia(new MafiaGame());
            SendMassage?.Invoke(user, true, new Answer(AnswerType.NewGame));
        }

        private void StartGame(User user, bool isCommonChat)
        {
            if (!isCommonChat) throw new ArgumentException("Игру можно начать только в чате");
            var userTeam = usersTeams[user.ComChatId];
            var mafia = usersTeams[user.ComChatId].Mafia;
            mafia.StartGame();
            var players = mafia.GetAllPlayers;
            foreach (var player in players)
            {
                var usr = userTeam.Users.First(u => u.Name == player.Name);
                SendMassage?.Invoke(usr, false, new Answer(GetRoleAnswerType(player)));
            }
            SendMassage?.Invoke(user, true, new Answer(AnswerType.GameStart));
        }

        private AnswerType GetRoleAnswerType(Player player)
        {
            if (player.Role.GetType() == typeof(PeacefulRole))
                return AnswerType.YouArePeaceful;
            if (player.Role.GetType() == typeof(MafiaRole))
                return AnswerType.YouAreMafia;
            throw new ArgumentException("Неопознанная роль");
        }

        private void RegPlayer(User user, bool isCommonChat)
        {
            if (!isCommonChat) throw new ArgumentException("Регестрироваться можно только в чате");
            var mafia = usersTeams[user.ComChatId].Mafia;
            var player = new Player(user.Name);
            mafia.RegisterPlayer(player);
            SendMassage?.Invoke(
                user,
                true,
                new Answer(AnswerType.SuccessfullyRegistered, new List<string> {user.Name}));
        }

        // private Answer KillPlayer(string killerName, string victimName)
        // {
        //     if (mafia.Status != Status.Night)
        //         return new Answer(true, AnswerType.DayKill);
        //
        //     var target = mafia.GetAllPlayers.First(x => x.Name == victimName);
        //     var killer = mafia.GetAllPlayers.First(x => x.Name == killerName);
        //     return killer.Role is MafiaRole 
        //         ? mafia.Kill(killer, target)
        //         : new Answer(true, AnswerType.NotMafia);
        // }
    }
}