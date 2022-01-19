using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class StartComHandler : AbstractCommandHandler
    {
        private readonly StartCommandInfo _info;
        public override void ExecuteCommand(GameTeam gT)
        {
            if (IsSend(!_info.IsComChat, new Answer(false, AnswerType.OnlyInCommon, _info.User.Name), _info.User.Id, _info.Service)) return;
            if (IsSend(gT.Mafia.Status is not Status.ReadyToStart,
                gT.Mafia.Status is Status.WaitingPlayers
                    ? new Answer(true, AnswerType.NeedMorePlayers, gT.Users.Select(u => u.Name).ToArray())
                    : new Answer(true, AnswerType.GameIsGoing), _info.ComChatId, _info.Service)) return;
            gT.Mafia.StartGame();
            var playersRoles = gT.Mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = gT.Users.First(u => u.Name == player);
                _send(new Answer(false, AnswerType.TellRole, 
                    GetToSend(playersRoles[usr.Name], playersRoles)), usr.Id, _info.Service);
            }
            _send(new Answer(true, AnswerType.GameStarted, gT.Users.Select(u => u.Name).ToArray()), gT.ChatId, _info.Service);
        }
        

        public StartComHandler(StartCommandInfo info, Action<Answer, ulong, string> send) : base(send)
        {
            _info = info;
        }

        private string GetToSend(Role playerRole, IReadOnlyDictionary<string, Role> playersRoles)
        {
            var toSend = playerRole.ToString();
            if (playerRole is MafiaRole)
            {
                toSend += "\nМафии в игре:" + playersRoles.Keys
                    .Where(player => playersRoles[player] is MafiaRole)
                    .Aggregate(toSend, (current, player) => "\n" + player);
            }
            return toSend;
        }
    }
}