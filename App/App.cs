using System.Linq;
using Mafia;
using Answers;

namespace App
{
    public class App
    {
        private readonly IMafia mafia;

        public App()
        {
            mafia = new MafiaGame();
        }

        public Answer ReproduceСommand (CommandContext ctx)
        {
            return ctx.command switch
            {
                RulesCommand rulesCommand => rulesCommand.GetRules(mafia),
                VoteCommand voteCommand => voteCommand.Vote(mafia, ctx.AuthorName, ctx.MentionedPlayers.First()),
                RegCommand regCommand => regCommand.RegPlayer(mafia, ctx.AuthorName),
                KillCommand killCommand => killCommand.KillPlayer(mafia, ctx.AuthorName, ctx.MentionedPlayers.First()),
                StartCommand startCommand => startCommand.StartGame(mafia),
                _ => new Answer(true, AnswerType.UnknownCommand)
            };
        }
    }
}