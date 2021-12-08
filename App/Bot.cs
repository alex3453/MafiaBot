using System;
using System.Collections.Generic;
using System.Linq;
using Mafia;
using CommonInteraction;

namespace App
{
    public class Bot 
    {
        private readonly IMafia mafia;

        public Bot(IMafia mafia)
        {
            this.mafia = mafia;
        }

        private void ReproduceCommand (Command ctx)
        {
            var ans = ctx.CommandType switch
            {
                CommandType.RulesCommand => GetRules(),
                CommandType.VoteCommand => Vote(ctx.AuthorName, ctx.MentionedPlayers.First()),
                CommandType.StartCommand => StartGame(),
                CommandType.RegCommand => RegPlayer(ctx.AuthorName),
                _ => new Answer(true, AnswerType.UnknownCommand)
            };

            Notify?.Invoke(ans);
        }

        private Answer Vote(string voterName, string targetName)
        {
            var target = mafia.GetAllPlayers.First(x => x.Name == targetName);
            var voter = mafia.GetAllPlayers.First(x => x.Name == voterName);
            var res = mafia.Vote(voter, target);
            return mafia.Status switch
            {
                Status.MafiaWins => new Answer(true, AnswerType.MafiaWins, new List<string> {mafia.Dead.Name}),
                Status.PeacefulWins => new Answer(true, AnswerType.PeacefulWins, new List<string> {mafia.Dead.Name}),
                _ => new Answer(true, res ? AnswerType.SuccessfullyVoted : AnswerType.UnsuccessfullyVoted)
            };
        }

        private Answer GetRules() => new(true, AnswerType.GetRules);

        private Answer StartGame()
        {
            mafia.StartGame();
            var ans = new Answer(true, AnswerType.GameStart);
            ans.SetArgs(mafia.GetAllPlayers.Select(player => $"{player.Name} {player.Role}").ToList());
            return ans;
        }

        private Answer RegPlayer(string name)
        {
            var player = new Player(name);
            mafia.RegisterPlayer(player);
            return new Answer(true, AnswerType.SuccessfullyRegistered, new List<string> {name});
        }

        // private Answer KillPlayer(string killerName, string victimName)
        // {
        //     if (mafia.Status != Status.Night)
        //         return new Answer(true, AnswerType.DayKill);
        //
        //     var target = mafia.GetAllPlayers.First(x => x.Name == victimName);
        //     var killer = mafia.GetAllPlayers.First(x => x.Name == killerName);
        //     return killer.Role is MafiaRole 
        //         ? mafia.Kill(killer, target)
        //         : new Answer(true, AnswerType.NotMafia);
        // }

        public Action<Command> Register() => ReproduceCommand;
        public event Action<Answer> Notify;
        
    }
}