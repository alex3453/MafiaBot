using System;
using System.Collections.Generic;
using CommonInteraction;
using Mafia;

namespace App
{
    public interface ICommandHandler
    {
        CommandType Type { get; }
        IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo);
    }
}