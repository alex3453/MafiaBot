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
        private readonly IAnswerParser _answerParser;
        
        public Action<User, bool, Answer> RegisterSending() => SendMessage;
        public void SubscribeOn(Action<User, bool, Command> exCommand) => _commandsHandler.ExCommand += exCommand;

        public View(
            DiscordSocketClient client,
            ICommandsHandler commandsHandler,
            ILogger logger,
            ITokenProvider provider, 
            IAnswerParser answerParser)
        {
            _client = client;
            _commandsHandler = commandsHandler;
            _logger = logger;
            _provider = provider;
            _answerParser = answerParser;
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += _commandsHandler.ProcessMessage;
            _client.Log += _logger.Log;
            await _client.LoginAsync(TokenType.Bot,  _provider.GetToken());
            await _client.StartAsync();
        }

        private void SendMessage(User user, bool isCommonChat, Answer answer)
        {
            if (isCommonChat)
            {
                var msgChannel = _client.GetChannel(user.CommonChannelId) as IMessageChannel;
                msgChannel?.SendMessageAsync(_answerParser.ParseAnswer(answer));
            }
            else
                _client.GetUser(user.Id).SendMessageAsync(_answerParser.ParseAnswer(answer));
        }
        
    }
}