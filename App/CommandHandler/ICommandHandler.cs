using System;
using System.Collections.Generic;
using CommonInteraction;
using Mafia;

namespace App
{
    public interface ICommandHandler
    {
        bool IsItMyCommand(CommandInfo commandInfo);
        IEnumerable<AnswerType> ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo);
    }
}