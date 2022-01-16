using System;
using System.Threading.Tasks;
using CommonInteraction;

namespace UserInterface
{
    public interface IView
    {
        public bool IsItMyService(Service service);
        public void Send(Answer answer, ulong destinationId);
        public void SubscribeOn(Action<ICommandInfo> exCommand);
        public Task StartAsync();
    }
}