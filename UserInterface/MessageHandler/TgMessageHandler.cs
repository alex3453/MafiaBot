using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UserInterface
{
    public class TgMessageHandler
    {
        private readonly IMessageParser _messageParser;
        public event Action<ICommandInfo> ExCommand;

        public TgMessageHandler(IMessageParser messageParser)
        {
            _messageParser = messageParser;
        }
        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (!_messageParser.Parse(CreateMessageData(update), out var commandInfo))
                return Task.CompletedTask;
            ExCommand?.Invoke(commandInfo);
            return Task.CompletedTask;
        }
        
        private MessageData CreateMessageData(Update update)
        {
            if (update.Type != UpdateType.Message)
                return null;
            if (update.Message!.Type != MessageType.Text)
                return null;
            var msg = update.Message;
            Console.WriteLine(msg.Text);
            var author = new Author(msg.From.IsBot, msg.From.Username, (ulong)msg.From.Id);
            var chat = msg.Chat;
            var isCommonChannel = chat.Type == ChatType.Group;
            var commonChannelId = isCommonChannel ? chat.Id : 0;
            var res = new MessageData(
                msg.Text,
                author,
                msg.Text.Split().Where(s => s.First() == '@').Select(s => s.Remove(0)).ToArray(),
                isCommonChannel,
                (ulong)commonChannelId
            );
            return res;
        }
    }
}