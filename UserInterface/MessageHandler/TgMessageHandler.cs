using System;
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
            // if (update.Type != UpdateType.Message)
            //     return Task.CompletedTask;
            // if (update.Message!.Type != MessageType.Text)
            //     return Task.CompletedTask;

            // var chatId = update.Message.Chat.Id;
            // var messageText = update.Message.Text;
            //
            // Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            //
            // _messageSender.SendMessage(new Answer(true, AnswerType.NewGame), (ulong)chatId);
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
            var a = new Author(false, "222", 44);
            return new MessageData("sss", a, new[] {"gg"}, true, 0);
            // var author = new Author(update., msg.Author.Username, msg.Author.Id);
            // var isCommonChannel = msg.Channel.GetType() == typeof(SocketTextChannel);
            // var commonChannelId = isCommonChannel ? msg.Channel.Id : 0;
            // var res = new MessageData(
            //     msg.Content,
            //     author,
            //     msg.MentionedUsers.Select(u => u.Username).ToArray(),
            //     isCommonChannel,
            //     commonChannelId
            // );
            // return res;
        }
    }
}