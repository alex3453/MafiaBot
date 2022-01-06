using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class VoteCommand : ICommandHandler
    {
        private readonly VoteCommandInfo _info;
        public VoteCommand(VoteCommandInfo info, Action<Answer, ulong> send) : base(send)
        {
            _info = info;
        }

        public override void ExecuteCommand(GameTeam gT)
        {
            if (IsSend(!_info.IsComChat,
                new Answer(false, AnswerType.OnlyInCommon, _info.User.Name), _info.User.Id)) return;
            if (IsSend(!gT.ContainsUser(_info.User),
                new Answer(true, AnswerType.YouAreNotInGame, _info.User.Name), _info.ComChatId)) return;
            if (IsSend(gT.Mafia.Status is not Status.Voting, 
                new Answer(true, AnswerType.NotTimeToVote, _info.User.Name), _info.ComChatId)) return;
            if (IsSend(!_info.MentPlayers.Any(), 
                new Answer(true, AnswerType.IncorrectVote, _info.User.Name), _info.ComChatId)) return;

            var voter = _info.User.Name;
            var target = _info.MentPlayers.First();
            var opStatus = gT.Mafia.Vote(voter, target);
            var answType = opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            _send(new Answer(true, answType, _info.User.Name, target), gT.ChatId);
            

            if (gT.Mafia.Status is Status.MafiaKilling)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    _send(new Answer(true, AnswerType.DayKill, gT.Mafia.Dead.ToArray()), gT.ChatId);
                else
                    _send(new Answer(true, AnswerType.DayAllAlive), gT.ChatId);
                _send(new Answer(true, AnswerType.EndDay), gT.ChatId);
                var killList = gT.Mafia.PlayersInGame
                    .Select((s, i) => (s, (i + 1).ToString()))
                    .SelectMany(t => new[] {t.s, t.Item2})
                    .ToArray();
                foreach (var player in gT.Mafia.MafiozyPlayers)
                {
                    var usr = gT.Users.First(u => u.Name == player);
                    _send(new Answer(false, AnswerType.MafiaKilling, killList), usr.Id);
                }

                return;
            }
            if (gT.Mafia.IsSomeBodyDied)
                _send(new Answer(true, AnswerType.DayKill, gT.Mafia.Dead.ToArray()), gT.ChatId);
            if (IsSend(gT.Mafia.Status is Status.MafiaWins, new Answer(true, AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId )) return;
            if (IsSend(gT.Mafia.Status is Status.PeacefulWins, new Answer(true, AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId )) return;
        }
    }
}