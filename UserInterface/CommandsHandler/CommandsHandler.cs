using System;
using System.Linq;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class CommandsHandler : ICommandsHandler
    {
        private readonly ICommandParser _commandParser;
        public event Action<CommandInfo> ExCommand;
        public event Action<User, bool, Answer> SendMassage;

        public CommandsHandler(ICommandParser commandParser)
        {
            _commandParser = commandParser;
        }

        public Task ProcessMessage(SocketMessage msg)
        {
            if (msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!') return Task.CompletedTask;
            var commandInfo = _commandParser.Parse(msg);
            if (commandInfo.CommandType == CommandType.Help)
            {
                SendMassage?.Invoke(commandInfo.User, 
                    commandInfo.IsCommonChat, 
                    new Answer(AnswerType.GetHelp, new []{ _commandParser.GetCommandsDescription() }));
                return Task.CompletedTask;
            }
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
            
        }
    }
}