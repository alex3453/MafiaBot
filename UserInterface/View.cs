﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using CommonInteraction;

namespace UserInterface
{
    public class View 
    {
        DiscordSocketClient client;
        public event Action<User, bool, Command> ExCommand;
        public Action<User, bool, Answer> RegisterSending() => SendMessage;

        private ITokenProvider provider;
        private IParserAnswers answersParser;

        public View(ITokenProvider provider, IParserAnswers parser)
        {
            this.provider = provider;
            answersParser = parser;
        }

        public async Task StartAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            var token = provider.GetToken();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            if (msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!') return Task.CompletedTask;
            var stringsCommand = msg.Content.Remove(0, 1).Split();
            var parser = new CommandParser(stringsCommand.First());
            var commandType = parser.Parse();
            var ctx = new Command(commandType, 
                msg.MentionedUsers.Select(x => x.Username).ToImmutableArray(), stringsCommand.Skip(1).ToList());
            var isCommonChat = msg.Channel.GetType() == typeof(SocketTextChannel);
            var user = new User(msg.Author.Id, msg.Channel.Id, msg.Author.Username);
            ExCommand?.Invoke(user, isCommonChat, ctx);
            return Task.CompletedTask;
        }

        private void SendMessage(User user, bool isCommonChat, Answer answer)
        {
            var msgChannel = client.GetChannel(user.CommonChannelId) as IMessageChannel;
            if (isCommonChat)
                msgChannel.SendMessageAsync(answersParser.ParseAnswer(answer));
            else
                client.GetUser(user.Id).SendMessageAsync(answersParser.ParseAnswer(answer));
        }
        
    }
}