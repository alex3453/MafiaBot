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

        private void CreateNewUsersTeam(User user)
        {
            usersTeams[user.CommonChannelId] = new UsersTeam();
        }

        private void ReproduceCommand (User user, bool isCommonChat, Command ctx)
        {
            if (!usersTeams.Keys.Contains(user.CommonChannelId))
                CreateNewUsersTeam(user);
            if (!usersTeams[user.CommonChannelId].IsContainsUser(user))
                usersTeams[user.CommonChannelId].AddUser(user);

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
                    DayControlVoting(user, isCommonChat, ctx.MentionedPlayers);
                    break;
                case CommandType.Kill:
                    NightControlKilling(user, isCommonChat, ctx.Content);
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

            var usersTeam = usersTeams[user.CommonChannelId];
            usersTeam.SetMafia();
            SendMassage?.Invoke(user, true, new Answer(AnswerType.NewGame));
        }
        
        private void RegPlayer(User user, bool isCommonChat)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }

            if (!usersTeams[user.CommonChannelId].IsMafiaSetted)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NeedToCreateGame));
                return;
            }
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            if (mafia.Status != Status.WaitingPlayers && mafia.Status != Status.ReadyToStart)
                SendMassage?.Invoke(user, true, new Answer(AnswerType.GameIsGoing));
            else if (mafia.AllPlayers.Contains(user.Name))
                SendMassage?.Invoke(user, true,
                    new Answer(AnswerType.AlreadyRegistered, new List<string> {user.Name}));
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
            if (!usersTeams[user.CommonChannelId].IsMafiaSetted)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NeedToCreateGame));
                return;
            }
            var userTeam = usersTeams[user.CommonChannelId];
            var mafia = usersTeams[user.CommonChannelId].Mafia;
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

        private void DayControlVoting(User user, bool isCommonChat, IEnumerable<string> mentionedPlayers)
        {
            Vote(user, isCommonChat, mentionedPlayers);
            if (!IsDayEnd(user)) return;
            if (!IsSomeBodyWin(user))
                EndDay(user);
        }

        private void NightControlKilling(User user, bool isCommonChat, IEnumerable<string> content)
        {
            KillPlayer(user, isCommonChat, content);
            if (!IsNightEnd(user)) return;
            if (!IsSomeBodyWin(user))
                EndNight(user);
        }

        private void Vote(User user, bool isCommonChat, IEnumerable<string> mentionedPlayers)
        {
            if (!isCommonChat)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.OnlyInCommon));
                return;
            }
            if (!usersTeams[user.CommonChannelId].IsMafiaSetted)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NeedToCreateGame));
                return;
            }

            var mafia = usersTeams[user.CommonChannelId].Mafia;
            string target;
            if (mafia.Status != Status.Voting)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NotTimeToVote));
                return;
            }
            if (!mentionedPlayers.Any())
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.IncorrectVote, new List<string> {user.Name}));
                return;
            }
            target = mentionedPlayers.First();
            if (!mafia.PlayersInGame.Contains(user.Name))
                SendMassage?.Invoke(user, true, new Answer(AnswerType.YouAreNotInGame,
                    new List<string> {user.Name}));
            else if (!mafia.PlayersInGame.Contains(target))
                SendMassage?.Invoke(user, true, new Answer(AnswerType.YouCantVoteThisPl,
                    new List<string> {user.Name, target}));
            else if (mafia.Vote(user.Name, target))
                SendMassage?.Invoke(user, true, new Answer(AnswerType.SuccessfullyVoted,
                    new List<string> {user.Name, target}));
            else
                SendMassage?.Invoke(user, true, new Answer(AnswerType.AlreadyVoted, new List<string>{user.Name}));
        }

        private void KillPlayer(User user, bool isCommonChat, IEnumerable<string> content)
        {
            if (isCommonChat)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.OnlyInLocal));
                return;
            }
            if (!usersTeams[user.CommonChannelId].IsMafiaSetted)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NeedToCreateGame));
                return;
            }
            
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            int target;
            if (mafia.Status != Status.MafiaKilling)
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.NotTimeToKill));
                return;
            }
            if (!content.Any() || !int.TryParse(content.First(), out target))
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.EnterNumber));
                return;
            }

            string targetName;
            if (mafia.PlayersNumbers.Values.Contains(target))
            {
                targetName = mafia.PlayersNumbers
                    .Where(kv => kv.Value == target)
                    .Select(kv => kv.Key)
                    .First();
            }
            else
            {
                SendMassage?.Invoke(user, false, new Answer(AnswerType.IncorrectNumber));
                return;
            }
            if (!mafia.PlayersInGame.Contains(user.Name))
                SendMassage?.Invoke(user, false, new Answer(AnswerType.YouAreNotInGame,
                    new List<string> {user.Name}));
            else if (!mafia.MafiozyPlayers.Contains(user.Name))
                SendMassage?.Invoke(user, false, new Answer(AnswerType.YouAreNotMafia));
            else if (!mafia.PlayersInGame.Contains(targetName))
                SendMassage?.Invoke(user, false, new Answer(AnswerType.YouCantKillThisPl,
                    new List<string> {targetName}));
            else if (mafia.Act(user.Name, targetName))
                SendMassage?.Invoke(user, false, new Answer(AnswerType.SuccessfullyKilled,
                    new List<string> {targetName}));
            else
                SendMassage?.Invoke(user, false, new Answer(AnswerType.AlreadyKilled));
        }

        private void EndDay(User user)
        {
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            if (mafia.IsSomeBodyDied)
                SendMassage?.Invoke(user, true, new Answer(AnswerType.DayKill,
                    mafia.Dead.ToList()));
            else
                SendMassage?.Invoke(user, true, new Answer(AnswerType.DayAllAlive));
            SendMassage?.Invoke(user, true, new Answer(AnswerType.EndDay));
            var userTeam = usersTeams[user.CommonChannelId];
            var mafiaKillList = new List<string>();
            foreach (var playerNumber in mafia.PlayersNumbers)
            {
                if (mafia.PlayersInGame.Contains(playerNumber.Key))
                {
                    mafiaKillList.Add(playerNumber.Key);
                    mafiaKillList.Add(playerNumber.Value.ToString());
                }
            }
            foreach (var player in mafia.MafiozyPlayers)
            {
                var usr = userTeam.Users.First(u => u.Name == player);
                SendMassage?.Invoke(usr, false, new Answer(AnswerType.MafiaKilling, mafiaKillList));
            }
        }

        private void EndNight(User user)
        {
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            if (mafia.IsSomeBodyDied)
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NightKill,
                    mafia.Dead.ToList()));
            else
                SendMassage?.Invoke(user, true, new Answer(AnswerType.NightAllAlive));
            SendMassage?.Invoke(user, true, new Answer(AnswerType.EndNight));
        }

        private bool IsDayEnd(User user)
        {
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            return mafia.Status is Status.MafiaKilling or Status.PeacefulWins or Status.MafiaWins;
        }

        private bool IsNightEnd(User user)
        {
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            return mafia.Status is Status.Voting or Status.PeacefulWins or Status.MafiaWins;
        }

        private bool IsSomeBodyWin(User user)
        {
            var mafia = usersTeams[user.CommonChannelId].Mafia;
            if (mafia.Status == Status.PeacefulWins)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.PeacefulWins));
                return true;
            }

            if (mafia.Status == Status.MafiaWins)
            {
                SendMassage?.Invoke(user, true, new Answer(AnswerType.MafiaWins));
                return true;
            }

            return false;
        }
    }
}