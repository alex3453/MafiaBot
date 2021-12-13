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
        private readonly ICommandsHandler _commandsHandler;
        private readonly ILogger _logger;
        private readonly ITokenProvider _provider;
        private readonly IMessageSender _messageSender;
        
        public Action<User, bool, Answer> RegisterSending() => _messageSender.SendMessage;
        public void SubscribeOn(Action<User, bool, CommandInfo> exCommand) => _commandsHandler.ExCommand += exCommand;

        public View(
            DiscordSocketClient client,
            ICommandsHandler commandsHandler,
            ILogger logger,
            ITokenProvider provider, 
            IMessageSender messageSender)
        {
            _client = client;
            _commandsHandler = commandsHandler;
            _logger = logger;
            _provider = provider;
            _messageSender = messageSender;
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += _commandsHandler.ProcessMessage;
            _client.Log += _logger.Log;
            await _client.LoginAsync(TokenType.Bot,  _provider.GetToken());
            await _client.StartAsync();
        }
    }
}