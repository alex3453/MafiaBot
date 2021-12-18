using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class KillCommand : ICommandHandler
    {
        private readonly KillCommandInfo info;

        public KillCommand(KillCommandInfo info)
        {
            this.info = info;
        }

        public override void ExecuteCommand(GameTeam gT, Action<Answer, ulong> send)
        {
            if (IsSend(gT is null, send,
                new Answer(false, AnswerType.YouAreNotInGame, info.User.Name), info.User.Id)) return;
            if (IsSend(info.IsComChat, send, 
                new Answer(true, AnswerType.OnlyInLocal, info.User.Name), info.ComChatId)) return;
            if (IsSend(!gT.ContainsUser(info.User), send, 
                new Answer(false, AnswerType.YouAreNotInGame, info.User.Name), info.User.Id)) return;
            if (IsSend(gT.Mafia.Status is not Status.MafiaKilling, send,
                new Answer(false, AnswerType.NotTimeToKill, info.User.Name), info.User.Id)) return;
            int target = 0;
            var isCorrect = !info.Content.Any() || !int.TryParse(info.Content.First(), out target);
            if (IsSend(isCorrect , send,
                new Answer(false, AnswerType.IncorrectNumber, info.User.Name), info.User.Id)) return;
            var killer = info.User.Name;
            var opStatus = gT.Mafia.Act(gT.Mafia.AllPlayers.Single(player => player.Name == killer), target);
            var answType =  opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            send(new Answer(false, answType, info.User.Name, target.ToString()), info.User.Id);

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