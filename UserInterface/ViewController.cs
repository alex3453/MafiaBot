using System;
using System.Linq;
using CommonInteraction;

namespace UserInterface
{
    public class ViewController
    {
        private readonly IView[] _views;

        public ViewController(IView[] views)
        {
            _views = views;
        }

        private void Send(Answer answer, ulong destinationId, Service service)
        {
            _views.FirstOrDefault(v => v.IsItMyService(service))?.Send(answer, destinationId);
        }

        public Action<Answer, ulong, Service> RegisterSending() => Send;

        public void SubscribeOn(Action<ICommandInfo> exCommand)
        {
            foreach (var view in _views)
            {
                view.SubscribeOn(exCommand);
            }
        }

        public void Start()
        {
            foreach (var view in _views)
            {
                view.StartAsync();
            }
        }
    }
}