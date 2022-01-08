using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class StartBaseCommand : BaseCommandHandler
    {
        private readonly StartCommandInfo _info;
        public override void ExecuteCommand(GameTeam gT)
        {
            if (IsSend(!_info.IsComChat, new Answer(false, AnswerType.OnlyInCommon, _info.User.Name), _info.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not Status.ReadyToStart,
                gT.Mafia.Status is Status.WaitingPlayers
                    ? new Answer(true, AnswerType.NeedMorePlayers, gT.Users.Select(u => u.Name).ToArray())
                    : new Answer(true, AnswerType.GameIsGoing), _info.ComChatId)) return;
            gT.Mafia.StartGame();
            var playersRoles = gT.Mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = gT.Users.First(u => u.Name == player);
                _send(new Answer(false, AnswerType.TellRole, 
                    GetToSend(playersRoles[usr.Name], playersRoles)), usr.Id);
            }
            _send(new Answer(true, AnswerType.GameStarted, gT.Users.Select(u => u.Name).ToArray()), gT.ChatId);
        }
        

        public StartBaseCommand(StartCommandInfo info, Action<Answer, ulong> send) : base(send)
        {
            _info = info;
        }

        private string GetToSend(Role playerRole, IReadOnlyDictionary<string, Role> playersRoles)
        {
            var toSend = playerRole.ToString();
            if (playerRole is MafiaRole)
            {
                toSend += "\nТвои напарники:" + playersRoles.Keys
                    .Where(player => playersRoles[player] is MafiaRole)
                    .Aggregate(toSend, (current, player) => current + "\n" + player);
            }
            return toSend;
        }
    }
}