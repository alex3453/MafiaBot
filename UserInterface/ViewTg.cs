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
        
        public ViewTg(
            ITokenProvider provider)
        {
            _provider = provider;
            // _messageSender = new TgSender(_client);
        }

        public Action<Answer, ulong> RegisterSending() => S;

        public void S(Answer a, ulong u){}

        public void SubscribeOn(Action<ICommandInfo> exCommand)
        {
        }

        public async Task StartAsync()
        {
            var botClient = new TelegramBotClient(_provider.GetToken());

            using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

// Send cancellation request to stop bot
            cts.Cancel();
        }

        // private void OnMessageHandler(object? sender, MessageEventArgs e)
        // {
        //     _messageSender.SendMessage(new Answer(true, AnswerType.OnlyInCommon), (ulong)e.Message.Chat.Id);
        // }
        
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
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