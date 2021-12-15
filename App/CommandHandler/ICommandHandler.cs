using System;
using System.Collections.Generic;
using CommonInteraction;
using Mafia;

namespace App
{
    public abstract class ICommandHandler
    {
        public abstract CommandType Type { get; }

        public abstract void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send);

        protected bool IsSend(bool toSend, Action<bool, Answer, ulong> send, 
            bool isCom, Answer answer, ulong id)
        {
            if (!toSend) return false;
            send(isCom, answer, id);
            return true;
        }
    }
}