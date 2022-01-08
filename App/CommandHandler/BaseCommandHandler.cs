using System;
using App.CommandHandler;
using CommonInteraction;

namespace App
{
    public abstract class BaseCommandHandler
    {
        protected readonly Action<Answer, ulong> _send;

        protected BaseCommandHandler(Action<Answer, ulong> send)
        {
            _send = send;
        }
        public abstract void ExecuteCommand(GameTeam gT);

        protected bool IsSend(bool toSend,
            Answer answer, ulong id)
        {
            if (!toSend) return false;
            _send(answer , id);
            return true;
        }
    }
}