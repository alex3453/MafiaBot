using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class ResetGameBaseCommand : BaseCommandHandler
    {
        private readonly ResetCommandInfo _info;
        public override void ExecuteCommand(GameTeam gT)
        {
            if (!_info.IsComChat)
                _send(new Answer(false, AnswerType.OnlyInCommon), _info.User.Id);
            else
            {
                gT.Reset();
                _send(new Answer(true, AnswerType.NewGame), _info.ComChatId);
            }
        }

        public ResetGameBaseCommand(ResetCommandInfo info, Action<Answer, ulong> send) : base( send)
        {
            _info = info;
        }
    }
}