using System;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App.CommandHandler
{
    public class VoteCommand : ICommandHandler
    {
        public override CommandType Type => CommandType.Vote;
        
        public override void ExecuteCommand(GameTeam gT, CommandInfo cI, Action<Answer, ulong> send)
        {
            if (IsSend(!cI.IsComChat, send, 
                new Answer(false, AnswerType.OnlyInCommon, cI.User.Name), cI.User.Id)) return;
            if (IsSend(!gT.ContainsUser(cI.User), send,
                new Answer(true, AnswerType.YouAreNotInGame, cI.User.Name), cI.ComChatId)) return;
            if (IsSend(gT.Mafia.Status is not Status.Voting, send, 
                new Answer(true, AnswerType.NotTimeToVote, cI.User.Name), cI.ComChatId)) return;
            if (IsSend(!cI.MentPlayers.Any(), send, 
                new Answer(true, AnswerType.IncorrectVote, cI.User.Name), cI.ComChatId)) return;

            var voter = cI.User.Name;
            var target = cI.MentPlayers.First();
            var opStatus = gT.Mafia.Vote(voter, target);
            var answType = opStatus switch
            {
                OperationStatus.Success => AnswerType.SuccessfullyVoted,
                OperationStatus.Already => AnswerType.AlreadyVoted,
                OperationStatus.Cant => AnswerType.YouCantVoteThisPl,
                OperationStatus.Incorrect => AnswerType.IncorrectVote,
                OperationStatus.NotInGame => AnswerType.YouAreNotInGame
            };
            send(new Answer(true, answType, cI.User.Name, target), gT.ChatId);
            

            if (gT.Mafia.Status is Status.MafiaKilling)
            {
                if (gT.Mafia.IsSomeBodyDied)
                    send(new Answer(true, AnswerType.DayKill, gT.Mafia.Dead.ToArray()), gT.ChatId);
                else
                    send(new Answer(true, AnswerType.DayAllAlive), gT.ChatId);
                send(new Answer(true, AnswerType.EndDay), gT.ChatId);
                var killList = gT.Mafia.PlayersInGame
                    .Select((s, i) => (s, (i + 1).ToString()))
                    .SelectMany(t => new[] {t.s, t.Item2})
                    .ToArray();
                foreach (var player in gT.Mafia.MafiozyPlayers)
                {
                    var usr = gT.Users.First(u => u.Name == player);
                    send(new Answer(false, AnswerType.MafiaKilling, killList), usr.Id);
                }

                return;
            }
            if (IsSend(gT.Mafia.Status is Status.MafiaWins, send,
                new Answer(true, AnswerType.MafiaWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId )) return;
            if (IsSend(gT.Mafia.Status is Status.PeacefulWins, send,
                new Answer(true, AnswerType.PeacefulWins, gT.Mafia.GetWinners().ToArray()), gT.ChatId )) return;
        }
    }
}