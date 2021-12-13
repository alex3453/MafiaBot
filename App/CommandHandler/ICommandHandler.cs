using System;
using CommonInteraction;
using Mafia;

namespace App
{
    public interface ICommandHandler
    {
        AnswerType ExecuteCommand(UsersTeam usersTeam, CommandInfo commandInfo);
    }
}