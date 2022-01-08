using System.Threading;
using CommonInteraction;
using Telegram.Bot;

namespace UserInterface
{
    public class TgSender : IMessageSender
    {
        private TelegramBotClient _client;
        private IAnswerParser _answerParser = new DefaultParser();
        private CancellationTokenSource _cts;

        public TgSender(CancellationTokenSource cts)
        {
            _cts = cts;
        }

        public void SetClient(TelegramBotClient client)
        {
            _client = client;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            {
                var res = _answerParser.ParseAnswer(answer);
                var sentMessage = _client.SendTextMessageAsync(
                    chatId: (long)destinationId,
                    text: res,
                    cancellationToken: _cts.Token);
            }
        }

        public void SetParser(IAnswerParser parser)
        {
            _answerParser = parser;
        }
    }
}