using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class HelpBaseCommand : BaseCommandHandler
    {
        private HelpCommandInfo _info;
        public HelpBaseCommand(HelpCommandInfo info, Action<Answer, ulong, Service> send) : base(send)
        {
            _info = info;
        }

        public override void ExecuteCommand(GameTeam gT)
        {
            _send(new Answer(_info.IsComChat,
                AnswerType.GetHelp), _info.IsComChat ? _info.ComChatId : _info.User.Id, _info.Service);
        }
    }
}