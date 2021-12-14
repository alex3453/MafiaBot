using System;
using CommonInteraction;

namespace App
{
    public interface IAnswerTypeHandler
    {
        bool IsItMyAnswerType(AnswerType answerType); 
        void SendMessage(AnswerType answerType, GameTeam gameTeam, CommandInfo commandInfo, Action<bool, Answer, ulong> send);
    }
}