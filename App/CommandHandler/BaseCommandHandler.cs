using System;
using App.CommandHandler;
using CommonInteraction;

namespace App
{
    public abstract class BaseCommandHandler
    {
        protected readonly Action<Answer, ulong, Service> _send;

        protected BaseCommandHandler(Action<Answer, ulong, Service> send)
        {
            _send = send;
        }
        public abstract void ExecuteCommand(GameTeam gT);

        protected bool IsSend(bool toSend,
            Answer answer, ulong id, Service service)
        {
            if (!toSend) return false;
            _send(answer, id, service);
            return true;
        }
    }
}