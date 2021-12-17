using System;
using App.CommandHandler;
using CommonInteraction;

namespace App
{
    public abstract class ICommandHandler : ICommand
    {
        public abstract void ExecuteCommand(GameTeam gT, Action<Answer, ulong> send);

        protected bool IsSend(bool toSend, Action<Answer, ulong> send, 
            Answer answer, ulong id)
        {
            if (!toSend) return false;
            send(answer , id);
            return true;
        }
    }
}