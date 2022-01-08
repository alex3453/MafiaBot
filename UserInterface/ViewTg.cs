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
    public class ViewTg : IView
    {
        private static TelegramBotClient _client;
        private readonly ITokenProvider _provider;
        private IMessageSender _messageSender;
        private readonly TgMessageHandler _messageHandler;
        private CancellationTokenSource _cts;

        public ViewTg(ITokenProvider provider, TgMessageHandler messageHandler)
        {
            _provider = provider;
            _messageHandler = messageHandler;
            _client = new TelegramBotClient(_provider.GetToken());
            _cts = new CancellationTokenSource();
            _messageSender = new TgSender(_client, _cts);
        }

        public Action<Answer, ulong> RegisterSending() => _messageSender.SendMessage;

        public void SubscribeOn(Action<ICommandInfo> exCommand)
        {
        }

        public async Task StartAsync()
        { 
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            _client.StartReceiving(
                _messageHandler.HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token);

            var me = await _client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            _cts.Cancel();
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}