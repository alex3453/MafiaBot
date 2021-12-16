using CommonInteraction;
using Discord;
using Discord.WebSocket;

namespace UserInterface
{
    public class MessageSender : IMessageSender
    {
        private readonly DiscordSocketClient _client;
        private readonly IAnswerParser _answerParser;

        public MessageSender(DiscordSocketClient client, IAnswerParser answerParser)
        {
            _client = client;
            _answerParser = answerParser;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            if (answer.IsCommon)
                ((IMessageChannel) _client.GetChannel(destinationId))
                    .SendMessageAsync(_answerParser.ParseAnswer(answer));
            else
                _client.GetUser(destinationId)
                    .SendMessageAsync(_answerParser.ParseAnswer(answer));
            
        }
    }
}