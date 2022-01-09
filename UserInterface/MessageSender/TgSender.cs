using System;
using System.Threading;
using CommonInteraction;
using Telegram.Bot;

namespace UserInterface
{
    public class TgSender : IMessageSender
    {
        private ITelegramBotClient _client;
        private IAnswerParser _answerParser = new DefaultParser();
        private readonly CancellationTokenSource _cts;

        public TgSender(CancellationTokenSource cts)
        {
            _cts = cts;
        }

        public void SetClient(ITelegramBotClient client)
        {
            _client = client;
        }
        
        public void SetParser(IAnswerParser parser)
        {
            _answerParser = parser;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            {
                var dest = MapUlongToLong(destinationId);
                var res = _answerParser.ParseAnswer(answer);
                var sentMessage = _client.SendTextMessageAsync(
                    chatId: dest,
                    text: res,
                    cancellationToken: _cts.Token);
            }
        }

        private static long MapUlongToLong(ulong ulongValue)
        {
            return unchecked((long)ulongValue + long.MinValue);
        }
    }
}