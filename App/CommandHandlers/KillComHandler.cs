using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class KillComHandler : AbstractCommandHandler
    {
        private KillCommandInfo _info;
        
        public override void ExecuteCommand(GameTeam gT)
        {
            if (IsSend(gT is null,
                new Answer(false, AnswerType.YouAreNotInGame, _info.User.Name), _info.User.Id, _info.Service)) return;
            if (IsSend(_info.IsComChat, 
                new Answer(true, AnswerType.OnlyInLocal, _info.User.Name), _info.ComChatId, _info.Service)) return;
            if (IsSend(!gT.ContainsUser(_info.User),
                new Answer(false, AnswerType.YouAreNotInGame, _info.User.Name), _info.User.Id, _info.Service)) return;
            if (IsSend(gT.Mafia.Status is not Status.MafiaKilling,
                new Answer(false, AnswerType.NotTimeToKill, _info.User.Name), _info.User.Id, _info.Service)) return;
            int target = 0;
            var isCorrect = !_info.Content.Any() || !int.TryParse(_info.Content.First(), out target);
            if (IsSend(isCorrect ,
                new Answer(false, AnswerType.IncorrectNumber, _info.User.Name), _info.User.Id, _info.Service)) return;
            var killer = _info.User.Name;
            var opStatus = gT.Mafia.Act(gT.Mafia.AllPlayers.Single(player => player.Name == killer), target);
            var answType =  opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyKilled,
                OperationStatus.Already => AnswerType.AlreadyKilled,
                OperationStatus.Cant => AnswerType.YouCantKillThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectNumber,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame,
                OperationStatus.WrongAct => AnswerType.YouAreNotMafia
            };
            _send(new Answer(false, answType, _info.User.Name, target.ToString()), _info.User.Id, _info.Service);

            if (gT.Mafia.Status is Status.Voting)
            {
                _send(gT.Mafia.IsSomeBodyDied
                        ? new Answer(true, AnswerType.NightKill, gT.Mafia.Dead.ToArray())
                        : new Answer(true, AnswerType.NightAllAlive), gT.ChatId, _info.Service);
                _send(new Answer(true, AnswerType.EndNight), gT.ChatId, _info.Service);
            }
            else if (gT.Mafia.Status is Status.MafiaWins)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    _send(new Answer(true, AnswerType.NightKill, gT.Mafia.Dead.ToArray()), gT.ChatId, _info.Service);
                _send(new Answer(true, AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId, _info.Service);
            }
            else if (gT.Mafia.Status is Status.PeacefulWins)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    _send(new Answer(true, AnswerType.NightKill, gT.Mafia.Dead.ToArray()), gT.ChatId, _info.Service);
                _send(new Answer(true, AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId, _info.Service);
            }
        }

        public KillComHandler(KillCommandInfo info, Action<Answer, ulong, string> send) : base(send)
        {
            _info = info;
        }
    }
}