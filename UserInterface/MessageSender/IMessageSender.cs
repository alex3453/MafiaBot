using CommonInteraction;

namespace UserInterface
{
    public interface IMessageSender
    {
        void SendMessage(User user, bool isCommonChat, Answer answer, ulong destinationId);
    }
}