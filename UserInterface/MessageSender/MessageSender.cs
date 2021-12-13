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

        public void SendMessage(User user, bool isCommonChat, Answer answer)
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