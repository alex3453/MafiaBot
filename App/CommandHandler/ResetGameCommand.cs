using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class ResetGameCommand : ICommandHandler
    {
        private readonly ResetCommandInfo info;

        public ResetGameCommand(ResetCommandInfo info)
        {
            this.info = info;
        }

        public override void ExecuteCommand(GameTeam gT, Action<Answer, ulong> send)
        {
            if (!info.IsComChat)
                send(new Answer(false, AnswerType.OnlyInCommon), info.User.Id);
            else
            {
                gT.Reset();
                send(new Answer(true, AnswerType.NewGame), info.ComChatId);
            }
        }
    }
}