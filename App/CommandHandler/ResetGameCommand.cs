using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class ResetGameCommand : ICommandHandler
    {
        public override CommandType Type => CommandType.CreateNewGame;
        
        public override void ExecuteCommand(GameTeam gT, CommandInfo cI,Action<Answer, ulong> send)
        {
            if (!cI.IsComChat)
                send(new Answer(false, AnswerType.OnlyInCommon), cI.User.Id);
            else
            {
                gT.Reset();
                send(new Answer(true, AnswerType.NewGame), cI.ComChatId);
            }
        }
    }
}