using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using CommonInteraction;

namespace UserInterface
{
    public class DsView : IView
    {
        private readonly DiscordSocketClient _client;
        private readonly DsMessageHandler _dsMessageHandler;
        private readonly IDsLogger _dsLogger;
        private readonly DsEnvVarProvider _provider;
        private readonly IMessageSender _messageSender;
        
        public DsView(
            DiscordSocketClient client,
            DsMessageHandler dsMessageHandler,
            IDsLogger dsLogger,
            DsEnvVarProvider provider, 
            IMessageSender messageSender)
        {
            _client = client;
            _dsMessageHandler = dsMessageHandler;
            _dsLogger = dsLogger;
            _provider = provider;
            _messageSender = messageSender;
        }
        
        public Action<Answer, ulong> RegisterSending() => _messageSender.SendMessage;
        public void SubscribeOn(Action<ICommandInfo> exCommand) => _dsMessageHandler.ExCommand += exCommand;

        public async Task StartAsync()
        {
            _client.MessageReceived += _dsMessageHandler.ProcessMessage;
            _client.Log += _dsLogger.Log;
            await _client.LoginAsync(TokenType.Bot,  _provider.GetToken());
            await _client.StartAsync();
        }
    }
}