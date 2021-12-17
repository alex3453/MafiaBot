using System;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class RegPlayerCommand : ICommandHandler
    {
        private readonly RegCommandInfo info;

        public RegPlayerCommand(RegCommandInfo info)
        {
            this.info = info;
        }

        public override void ExecuteCommand(GameTeam gT, Action<Answer, ulong> send)
        {
            if (IsSend(!info.IsComChat, send,
                new Answer(false, AnswerType.OnlyInCommon, info.User.Name), info.User.Id)) return;
            if (IsSend(gT.ContainsUser(info.User), send,
                new Answer(true, AnswerType.AlreadyRegistered, info.User.Name), info.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not (Status.WaitingPlayers or Status.ReadyToStart), send, 
                new Answer(true, AnswerType.GameIsGoing, info.User.Name), info.ComChatId)) return;
            gT.AddUser(info.User);
            send(new Answer(true, AnswerType.SuccessfullyRegistered, info.User.Name), info.ComChatId);
        }
    }
}