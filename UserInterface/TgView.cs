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
        private readonly TgEnvVarTokenProvider _provider;
        private readonly TgSender _messageSender;
        private readonly TgMessageHandler _messageHandler;
        private readonly ITgErrorHandler _errorHandler;
        private readonly CancellationTokenSource _cts;

        public TgView(
            TgEnvVarTokenProvider provider, 
            TgMessageHandler messageHandler, 
            IMessageSender messageSender,
            ITgErrorHandler errorHandler,
            CancellationTokenSource cts)
        {
            _provider = provider;
            _messageHandler = messageHandler;
            _errorHandler = errorHandler;
            _messageSender = messageSender as TgSender;
            _cts = cts;
        }

        public Action<Answer, ulong> RegisterSending() => _messageSender.SendMessage;

        public void SubscribeOn(Action<ICommandInfo> exCommand) => _messageHandler.ExCommand += exCommand;

        private void SetUp()
        {
            _client = new TelegramBotClient(_provider.GetToken());
            _messageSender.SetClient(_client);
            _messageHandler.SetSender(_messageSender);
        }

        public async Task StartAsync()
        {
            SetUp();
            
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            _client.StartReceiving(
                _messageHandler.HandleUpdateAsync,
                _errorHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken: _cts.Token);

            var me = await _client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            _cts.Cancel();
        }
    }
}