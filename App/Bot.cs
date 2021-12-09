using System;
using System.Collections.Generic;
using System.Linq;
using Mafia;
using CommonInteraction;

namespace App
{
    public class Bot 
    {
        private IMafia mafia;
        

        public Answer StartNewGame()
        {
            mafia = new MafiaGame();
            return new Answer(true, AnswerType.NewGame);
        }

        private void ReproduceCommand (Command ctx)
        {
            var ans = ctx.CommandType switch
            {
                CommandType.Rules => GetRules(),
                CommandType.Vote => Vote(ctx.AuthorName, ctx.MentionedPlayers.First()),
                CommandType.Start => StartGame(),
                CommandType.Reg => RegPlayer(ctx.AuthorName),
                CommandType.StartNewGame => StartNewGame(),
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
            var ans = new Answer(true, AnswerType.GameStart, 
                mafia.GetAllPlayers.ToDictionary(player => player.Name, player => player.Role.ToString()));
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