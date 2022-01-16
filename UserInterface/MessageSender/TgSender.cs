using System;
using System.Threading;
using CommonInteraction;
using Telegram.Bot;

namespace UserInterface
{
    public class TgSender : IMessageSender
    {
        private TelegramBotClient _client;
        private IAnswerGenerator _answerGenerator = new DefaultGenerator();

        public TgSender(TelegramBotClient client)
        {
            _client = client;
        }

        public void SetParser(IAnswerGenerator generator)
        {
            _answerGenerator = generator;
        }

        public bool IsItMyService(Service service)
        {
            return service == Service.Telegram;
        }

        public void SendMessage(Answer answer, ulong destinationId)
        {
            var dest = MapUlongToLong(destinationId);
            var res = _answerGenerator.GenerateAnswer(answer);
            _client.SendTextMessageAsync(dest, res);
        }

        private static long MapUlongToLong(ulong ulongValue)
        {
            return unchecked((long)ulongValue + long.MinValue);
        }
    }
}