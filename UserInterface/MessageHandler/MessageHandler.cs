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
        public event Action<ICommandInfo> ExCommand;

        public MessageHandler(IMessageParser messageParser)
        {
            _messageParser = messageParser;
        }

        public Task ProcessMessage(SocketMessage msg)
        {
            if (!_messageParser.Parse(msg, out var commandInfo))
                return Task.CompletedTask;
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
            
        }
    }
}