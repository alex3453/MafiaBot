using System;
using CommonInteraction;

namespace App
{
    public interface IAnswerTypeHandler
    {
        bool IsItMyAnswerType(AnswerType answerType); 
        void SendMessage(AnswerType answerType, UsersTeam usersTeam, CommandInfo commandInfo, Action<User, bool, Answer> send);
    }
}