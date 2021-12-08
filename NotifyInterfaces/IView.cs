using System;
using CommonInteraction;

namespace NotifyInterfaces
{
    public interface IView
    {
        void Run();
        event Func<Command, Answer> Notify;
    }
}