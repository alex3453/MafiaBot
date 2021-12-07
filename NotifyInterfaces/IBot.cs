using System;
using Answers;
using Commands;

namespace NotifyInterfaces
{
    public interface IBot
    {
        void Run();
        event Func<Command, Answer> Notify;
    }
}