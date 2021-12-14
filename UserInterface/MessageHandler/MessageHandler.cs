using System;
using System.Linq;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IMessageParser _messageParser;
        public event Action<CommandInfo> ExCommand;
        public event Action<bool, Answer, ulong> SendMassage;

        public MessageHandler(IMessageParser messageParser)
        {
            _messageParser = messageParser;
        }

        public Task ProcessMessage(SocketMessage msg)
        {
            if (msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!') return Task.CompletedTask;
            var commandInfo = _messageParser.Parse(msg);
            if (commandInfo.CommandType == CommandType.Help)
            {
                SendMassage?.Invoke(commandInfo.IsCommonChannel,
                    new Answer(AnswerType.GetHelp, new []{ _messageParser.GetCommandsDescription() }), msg.Channel.Id);
                return Task.CompletedTask;
            }
            if (commandInfo.CommandType == CommandType.Unknown)
            {
                SendMassage?.Invoke(commandInfo.IsCommonChannel, new Answer(AnswerType.Unknown), msg.Channel.Id);
                return Task.CompletedTask;
            }
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
            
        }
    }
}