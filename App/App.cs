using System.Collections.Generic;
using System.Linq;
using Mafia;
using Answers;
using Commands;

namespace App
{
    public class App
    {
        private readonly IMafia mafia;

        public App()
        {
            mafia = new MafiaGame();
        }

        public Answer ReproduceСommand (Command ctx)
        {
            return ctx.CommandType switch
            {
                CommandType.RulesCommand => GetRules(),
                CommandType.VoteCommand => Vote(ctx.AuthorName, ctx.MentionedPlayers.First()),
                CommandType.StartCommand => StartGame(),
                CommandType.RegCommand => RegPlayer(ctx.AuthorName),
                CommandType.KillCommand => KillPlayer(ctx.AuthorName, ctx.MentionedPlayers.First()),
                _ => new Answer(true, AnswerType.UnknownCommand),
            };
        }

        private Answer Vote(string voterName, string targetName)
        {
            var target = mafia.GetAllPlayers.First(x => x.Name == targetName);
            var voter = mafia.GetAllPlayers.First(x => x.Name == voterName);
            return mafia.Vote(voter, target);
        }

        private Answer GetRules()
        {
            return mafia.GetRules();
        }

        private Answer StartGame()
        {
            var ans = mafia.StartGame();
            ans.SetArgs(mafia.GetAllPlayers.Select(player => $"{player.Name} {player.Role}").ToList());
            return ans;
        }

        private Answer RegPlayer(string name)
        {
            var player = new Player(name);
            mafia.RegisterPlayer(player);
            return new Answer(true, AnswerType.SuccessfullyRegistered, new List<string> {name});
        }

        private Answer KillPlayer(string killerName, string victimName)
        {
            if (mafia.Status != Status.Night)
                return new Answer(true, AnswerType.DayKill);

            var target = mafia.GetAllPlayers.First(x => x.Name == victimName);
            var killer = mafia.GetAllPlayers.First(x => x.Name == killerName);
            return killer.Role is MafiaRole 
                ? mafia.Kill(killer, target)
                : new Answer(true, AnswerType.NotMafia);
        }
        
    }
}