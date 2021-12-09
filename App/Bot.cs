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
        
        public Action<User, Command> Register() => ReproduceCommand;
        public event Action<User, Answer> ExCommand;
        public event Action<User> DeleteUser; 

        public Answer StartNewGame()
        {
            mafia = new MafiaGame();
            return new Answer(AnswerType.NewGame);
        }

        private void ReproduceCommand (User user, Command ctx)
        {
            var ans = ctx.CommandType switch
            {
                CommandType.Rules => GetRules(),
                CommandType.Vote => Vote(ctx.AuthorName, ctx.MentionedPlayers.First()),
                CommandType.Start => StartGame(),
                CommandType.Reg => RegPlayer(ctx.AuthorName),
                CommandType.StartNewGame => StartNewGame(),
                _ => new Answer(AnswerType.UnknownCommand)
            };

            ExCommand?.Invoke(ans);
        }

        private Answer Vote(string voterName, string targetName)
        {
            var target = mafia.GetAllPlayers.First(x => x.Name == targetName);
            var voter = mafia.GetAllPlayers.First(x => x.Name == voterName);
            var res = mafia.Vote(voter, target);
            return mafia.Status switch
            {
                Status.MafiaWins => new Answer(AnswerType.MafiaWins, new List<string> {mafia.Dead.Name}),
                Status.PeacefulWins => new Answer(AnswerType.PeacefulWins, new List<string> {mafia.Dead.Name}),
                _ => new Answer(res ? AnswerType.SuccessfullyVoted : AnswerType.UnsuccessfullyVoted)
            };
        }

        private Answer GetRules() => new(AnswerType.GetRules);

        private Answer StartGame()
        {
            mafia.StartGame();
            var ans = new Answer(AnswerType.GameStart, 
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
    }
}