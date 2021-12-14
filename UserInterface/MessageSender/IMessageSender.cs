using CommonInteraction;

namespace UserInterface
{
    public interface IMessageSender
    {
        void SendMessage(bool isCommonChat, Answer answer, ulong destinationId);
    }
}