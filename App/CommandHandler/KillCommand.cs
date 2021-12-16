using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class KillCommand : ICommandHandler
    {
        public override CommandType Type => CommandType.Kill;
        
        public override void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<Answer, ulong> send)
        {
            if (IsSend(gT is null, send,
                new Answer(false, AnswerType.YouAreNotInGame, cI.User.Name), cI.User.Id)) return;
            if (IsSend(cI.IsComChat, send, 
                new Answer(true, AnswerType.OnlyInLocal, cI.User.Name), cI.ComChatId)) return;
            if (IsSend(!gT.ContainsUser(cI.User), send, 
                new Answer(false, AnswerType.YouAreNotInGame, cI.User.Name), cI.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not Status.MafiaKilling, send,
                new Answer(false, AnswerType.NotTimeToKill, cI.User.Name), cI.User.Id)) return;
            int target = 0;
            var isCorrect = !cI.Content.Any() || !int.TryParse(cI.Content.First(), out target);
            if (IsSend(isCorrect , send,
                new Answer(false, AnswerType.IncorrectNumber, cI.User.Name), cI.User.Id)) return;
            var killer = cI.User.Name;
            var opStatus = gT.Mafia.Act(killer, target);
            var answType =  opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            send(new Answer(false, answType, cI.User.Name, target.ToString()), cI.User.Id);

            if (gT.Mafia.Status is Status.Voting)
            {
                send(gT.Mafia.IsSomeBodyDied
                        ? new Answer(true, AnswerType.NightKill, gT.Mafia.Dead.ToArray())
                        : new Answer(true, AnswerType.NightAllAlive), gT.ChatId);
                send(new Answer(true, AnswerType.EndNight), gT.ChatId);
            }
            else if (gT.Mafia.Status is Status.MafiaWins)
                send(new Answer(true, AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
            else if (gT.Mafia.Status is Status.PeacefulWins)
                send(new Answer(true, AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId);
        }
    }
}