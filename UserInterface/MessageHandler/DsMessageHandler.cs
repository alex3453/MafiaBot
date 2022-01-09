using System;
using System.Linq;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class DsMessageHandler
    {
        private readonly MessageParser _messageParser;
        public event Action<ICommandInfo> ExCommand;

        public DsMessageHandler(MessageParser messageParser)
        {
            _messageParser = messageParser;
        }

        public Task ProcessMessage(SocketMessage msg)
        {
            if (!_messageParser.Parse(CreateMessageData(msg), out var commandInfo))
                return Task.CompletedTask;
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
        }

        private MessageData CreateMessageData(SocketMessage msg)
        {
            var author = new Author(msg.Author.IsBot, msg.Author.Username, msg.Author.Id);
            var isCommonChannel = msg.Channel.GetType() == typeof(SocketTextChannel);
            var commonChannelId = isCommonChannel ? msg.Channel.Id : 0;
            var res = new MessageData(
                msg.Content,
                author,
                msg.MentionedUsers.Select(u => u.Username).ToArray(),
                isCommonChannel,
                commonChannelId
                );
            return res;
        }
    }
}