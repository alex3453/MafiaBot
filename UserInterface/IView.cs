using System;
using System.Threading.Tasks;
using CommonInteraction;

namespace UserInterface
{
    public interface IView
    {
        public Action<Answer, ulong> RegisterSending();
        public void SubscribeOn(Action<ICommandInfo> exCommand);
        public Task StartAsync();
    }
}