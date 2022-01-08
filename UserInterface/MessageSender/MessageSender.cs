using System.Linq;
using CommonInteraction;
using Discord;
using Discord.WebSocket;

namespace UserInterface
{
    public class MessageSender : IMessageSender
    {
        private readonly DiscordSocketClient _client;
        private IAnswerParser _answerParser = new DefaultParser();

        public MessageSender(DiscordSocketClient client)
        {
            _client = client;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            if (answer.IsCommon)
            {
                var channel = (SocketTextChannel)_client.GetChannel(destinationId);
                channel.SendMessageAsync(_answerParser.ParseAnswer(answer));
            }
            else
                _client.GetUser(destinationId)
                    .SendMessageAsync(_answerParser.ParseAnswer(answer));
            
        }

        public void SetParser(IAnswerParser parser)
        {
            _answerParser = parser;
        }
    }
}