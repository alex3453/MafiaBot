using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using CommonInteraction;

namespace UserInterface
{
    public class View
    {
        private readonly DiscordSocketClient _client;
        private readonly IMessageHandler _messageHandler;
        private readonly ILogger _logger;
        private readonly ITokenProvider _provider;
        private readonly IMessageSender _messageSender;
        
        public Action<Answer, ulong> RegisterSending() => _messageSender.SendMessage;
        public void SubscribeOn(Action<CommandInfo> exCommand) => _messageHandler.ExCommand += exCommand;

        public View(
            DiscordSocketClient client,
            IMessageHandler messageHandler,
            ILogger logger,
            ITokenProvider provider, 
            IMessageSender messageSender)
        {
            _client = client;
            _messageHandler = messageHandler;
            _logger = logger;
            _provider = provider;
            _messageSender = messageSender;
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += _messageHandler.ProcessMessage;
            _client.Log += _logger.Log;
            _messageHandler.SendMassage += RegisterSending();
            await _client.LoginAsync(TokenType.Bot,  _provider.GetToken());
            await _client.StartAsync();
        }
    }
}