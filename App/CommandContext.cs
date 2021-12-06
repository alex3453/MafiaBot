using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using Discord.WebSocket;
using DiscordBot.Mafia;

namespace DiscordBot.Application
{
    public class CommandContext
    {
        public ICommand command { get; }
        public ulong AuthorId { get; }
        public string AuthorName { get; }
        public IReadOnlyCollection<string> MentionedPlayers { get; }

        public CommandContext(ICommand command, ulong authorId, 
            IReadOnlyCollection<string> mentionedPlayers, string authorName)
        {
            this.command = command;
            AuthorId = authorId;
            MentionedPlayers = mentionedPlayers;
            AuthorName = authorName;
        }
    }

    public class VoteCommand : ICommand
    {
        public Answer Vote(IMafia mafia, string voterName, string targetName)
        {
            var target = mafia.GetAllPlayers.First(x => x.Name == targetName);
            var voter = mafia.GetAllPlayers.First(x => x.Name == voterName);
            return mafia.Vote(voter, target);
        }
    }

    public class RulesCommand : ICommand
    {
        public Answer GetRules(IMafia mafia)
        {
            return mafia.GetRules();
        }
    }
    
    public class StartCommand : ICommand
    {
        public Answer StartGame(IMafia mafia)
        {
            var ans = mafia.StartGame();
            ans.SetArgs(mafia.GetAllPlayers.Select(player => $"{player.Name} {player.Role}").ToList());
            return ans;
        }

    }

    public class RegCommand : ICommand
    {
        public Answer RegPlayer(IMafia mafia, string name)
        {
            var player = new Player(name);
            mafia.RegisterPlayer(player);
            return new Answer(true, Answers.SuccessfullyRegistered, new List<string> {name});
        }
    }

    public class KillCommand : ICommand
    {
        public Answer KillPlayer(IMafia mafia, string killerName, string victimName)
        {
            if (mafia.Status != Status.Night)
                return new Answer(true, Answers.DayKill);

            var target = mafia.GetAllPlayers.First(x => x.Name == victimName);
            var killer = mafia.GetAllPlayers.First(x => x.Name == killerName);
            return !(killer.Role is MafiaRole) ? new Answer(true, Answers.NotMafia) : mafia.Kill(killer, target);
        }
    }
}