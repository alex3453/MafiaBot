using System;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class RegPlayerCommand : ICommandHandler
    {
        public override CommandType Type => CommandType.Reg;
        
        public override void ExecuteCommand(GameTeam gT,CommandInfo cI, Action<Answer, ulong> send)
        {
            if (IsSend(!cI.IsComChat, send,
                new Answer(false, AnswerType.OnlyInCommon, cI.User.Name), cI.User.Id)) return;
            if (IsSend(gT.ContainsUser(cI.User), send,
                new Answer(true, AnswerType.AlreadyRegistered, cI.User.Name), cI.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not (Status.WaitingPlayers or Status.ReadyToStart), send, 
                new Answer(true, AnswerType.GameIsGoing, cI.User.Name), cI.ComChatId)) return;
            gT.AddUser(cI.User);
            send(new Answer(true, AnswerType.SuccessfullyRegistered, cI.User.Name), cI.ComChatId);
        }
    }
}