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
        private readonly MessageParser _messageParser;
        private TgSender _sender;
        public event Action<ICommandInfo> ExCommand;

        public TgMessageHandler(MessageParser messageParser)
        {
            _messageParser = messageParser;
        }

        public void SetSender(TgSender sender)
        {
            _sender = sender;
        }
        
        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(update.Message.Text);
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
            var author = new Author(msg.From.IsBot, msg.From.Username, MapLongToUlong(msg.From.Id));
            var chat = msg.Chat;
            var isCommonChannel = chat.Type == ChatType.Group;
            var commonChannelId = isCommonChannel ? chat.Id : 0;
            var mentionedUsers = Array.Empty<string>();
            if (msg.Text != null)
                mentionedUsers = msg.Text.Split().Where(s => s.First() == '@').Select(s => s.Remove(0, 1)).ToArray();

            var res = new MessageData(
                msg.Text,
                author,
                mentionedUsers,
                isCommonChannel,
                MapLongToUlong(commonChannelId)
            );
            return res;
        }
        
        private ulong MapLongToUlong(long longValue)
        {
            return unchecked((ulong)(longValue - long.MinValue));
        }
    }
}