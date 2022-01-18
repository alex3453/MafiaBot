using System;
using System.Threading;
using System.Threading.Tasks;
using CommonInteraction;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UserInterface
{
    public class TgView : IView
    {
        private TelegramBotClient _client;
        private readonly TgSender _messageSender;
        private readonly TgMessageHandler _messageHandler;
        private readonly ITgErrorHandler _errorHandler;

        public TgView(
            TgMessageHandler messageHandler, 
            TgSender messageSender,
            ITgErrorHandler errorHandler,
            TelegramBotClient client)
        {
            _messageHandler = messageHandler;
            _errorHandler = errorHandler;
            _messageSender = messageSender;
            _client = client;
        }

        public bool IsItMyService(string service)
        {
            return service == "Telegram";
        }

        public void Send(Answer answer, ulong destinationId)
        {
            _messageSender.SendMessage(answer, destinationId);
        }

        public void SubscribeOn(Action<ICommandInfo> exCommand) => _messageHandler.ExCommand += exCommand;

        public async Task StartAsync()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            _client.StartReceiving(
                _messageHandler.HandleUpdateAsync,
                _errorHandler.HandleErrorAsync,
                receiverOptions);

            var me = await _client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
        }
    }
}