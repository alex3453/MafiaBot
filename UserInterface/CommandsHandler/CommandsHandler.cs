using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class CommandsHandler : ICommandsHandler
    {
        public event Action<User, bool, Command> ExCommand;
        
        public Task ProcessMessage(SocketMessage msg)
        {
            if (msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!') return Task.CompletedTask;
            var stringsCommand = msg.Content.Remove(0, 1).Split();
            var parser = new CommandParser(stringsCommand.First());
            var commandType = parser.Parse();
            var ctx = new Command(commandType, 
                msg.MentionedUsers.Select(x => x.Username).ToImmutableArray(), stringsCommand.Skip(1).ToList());
            var isCommonChat = msg.Channel.GetType() == typeof(SocketTextChannel);
            var user = new User(msg.Author.Id, msg.Channel.Id, msg.Author.Username);
            ExCommand?.Invoke(user, isCommonChat, ctx);
            return Task.CompletedTask;
        }
    }
}