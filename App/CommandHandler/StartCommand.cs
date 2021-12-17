using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class StartCommand : ICommandHandler
    {
        private readonly StartCommandInfo info;

        public StartCommand(StartCommandInfo info)
        {
            this.info = info;
        }

        public override void ExecuteCommand(GameTeam gT, Action<Answer, ulong> send)
        {
            if (IsSend(!info.IsComChat, send, new Answer(false, AnswerType.OnlyInCommon, info.User.Name), info.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not Status.ReadyToStart, send,
                gT.Mafia.Status is Status.WaitingPlayers
                    ? new Answer(true, AnswerType.NeedMorePlayers, gT.Users.Select(u => u.Name).ToArray())
                    : new Answer(true, AnswerType.GameIsGoing), info.ComChatId)) return;
            gT.Mafia.StartGame();
            var playersRoles = gT.Mafia.PlayersRoles;
            foreach (var player in playersRoles.Keys)
            {
                var usr = gT.Users.First(u => u.Name == player);
                send(new Answer(false, AnswerType.TellRole, playersRoles[usr.Name].ToString()), usr.Id);
            }
            send(new Answer(true, AnswerType.GameStarted, gT.Users.Select(u => u.Name).ToArray()), gT.ChatId);
        }
        
    }
}