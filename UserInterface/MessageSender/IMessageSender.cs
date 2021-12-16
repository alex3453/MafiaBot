using CommonInteraction;

namespace UserInterface
{
    public interface IMessageSender
    {
        void SendMessage(Answer answer, ulong destinationId);
    }
}