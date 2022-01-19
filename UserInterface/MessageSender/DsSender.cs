using System;
using System.Linq;
using CommonInteraction;
using Discord;
using Discord.WebSocket;

namespace UserInterface
{
    public class DsSender : IMessageSender
    {
        private readonly DiscordSocketClient _client;
        private AnswerGenerator _answerGenerator = new DefaultGenerator();

        public DsSender(DiscordSocketClient client)
        {
            _client = client;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            if (answer.IsCommon)
            {
                var channel = (SocketTextChannel)_client.GetChannel(destinationId);
                channel.SendMessageAsync(_answerGenerator.GenerateAnswer(answer));
            }
            else
                _client.GetUser(destinationId)
                    .SendMessageAsync(_answerGenerator.GenerateAnswer(answer));
            
        }

        public void SetParser(AnswerGenerator generator)
        {
            _answerGenerator = generator;
        }

        public bool IsItMyService(Service service)
        {
            return service == Service.Discord;
        }
    }
}