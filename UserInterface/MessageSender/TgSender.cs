using CommonInteraction;
using Telegram.Bot;

namespace UserInterface
{
    public class TgSender : IMessageSender
    {
        private readonly TelegramBotClient _client;
        private IAnswerParser _answerParser = new DefaultParser();

        public TgSender(TelegramBotClient client)
        {
            _client = client;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            _client.SendTextMessageAsync(destinationId is long ? (long) destinationId : 0, _answerParser.ParseAnswer(answer));
        }

        public void SetParser(IAnswerParser parser)
        {
            _answerParser = parser;
        }
    }
}