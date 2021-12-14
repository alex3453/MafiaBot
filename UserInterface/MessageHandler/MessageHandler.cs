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
            if (!_messageParser.Parse(msg, out var commandInfo))
                return Task.CompletedTask;
            if (commandInfo.CommandType == CommandType.Help)
            {
                SendMassage?.Invoke(
                    commandInfo.IsComChat,
                    new Answer(
                        AnswerType.GetHelp, 
                        new []{ _messageParser.GetCommandsDescription() }), 
                    commandInfo.IsComChat ? commandInfo.ComChatId : commandInfo.User.Id);
                return Task.CompletedTask;
            }
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
            
        }
    }
}