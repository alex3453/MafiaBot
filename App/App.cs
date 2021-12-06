using System.Linq;
using DiscordBot.Mafia;

namespace DiscordBot.Application
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
                _ => new Answer(true, Answers.UnknownCommand)
            };
        }
    }
}