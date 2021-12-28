using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class HelpCommand : ICommandHandler
    {
        private HelpCommandInfo _info;
        public HelpCommand(HelpCommandInfo info, Action<Answer, ulong> send) : base(send)
        {
            _info = info;
        }

        public override void ExecuteCommand(GameTeam gT)
        {
            _send(new Answer(_info.IsComChat,
                AnswerType.GetHelp), _info.IsComChat ? _info.ComChatId : _info.User.Id);
        }
    }
}