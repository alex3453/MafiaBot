using System;
using System.Collections.Generic;
using CommonInteraction;
using Mafia;

namespace App
{
    public interface ICommandHandler
    {
        CommandType Type { get; }
        void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<bool, Answer, ulong> send);
    }
}