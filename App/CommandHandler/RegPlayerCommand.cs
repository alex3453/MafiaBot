using System;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class RegPlayerCommand : ICommandHandler
    {
        private RegCommandInfo _info;
        public override void ExecuteCommand(GameTeam gT)
        {
            if (IsSend(!_info.IsComChat,
                new Answer(false, AnswerType.OnlyInCommon, _info.User.Name), _info.User.Id)) return;
            if (IsSend(gT.ContainsUser(_info.User),
                new Answer(true, AnswerType.AlreadyRegistered, _info.User.Name), _info.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not (Status.WaitingPlayers or Status.ReadyToStart), 
                new Answer(true, AnswerType.GameIsGoing, _info.User.Name), _info.ComChatId)) return;
            gT.AddUser(_info.User);
            _send(new Answer(true, AnswerType.SuccessfullyRegistered, _info.User.Name), _info.ComChatId);
        }

        public RegPlayerCommand(RegCommandInfo info, Action<Answer, ulong> send) : base(send)
        {
            _info = info;
        }
    }
}