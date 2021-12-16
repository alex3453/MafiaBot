using System;
using System.Collections.Generic;
using CommonInteraction;
using Mafia;

namespace App
{
    public abstract class ICommandHandler
    {
        public abstract CommandType Type { get; }

        public abstract void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<Answer, ulong> send);

        protected bool IsSend(bool toSend, Action<Answer, ulong> send, 
            Answer answer, ulong id)
        {
            if (!toSend) return false;
            send(answer , id);
            return true;
        }
    }
}