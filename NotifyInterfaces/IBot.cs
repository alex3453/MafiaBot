using System;
using CommonInteraction;

namespace NotifyInterfaces
{
    public interface IBot
    {
        Func<Command, Answer> Register();
    }
}