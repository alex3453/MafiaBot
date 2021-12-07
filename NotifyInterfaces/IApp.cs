using System;
using Answers;
using Commands;

namespace NotifyInterfaces
{
    public interface IApp
    {
        Func<Command, Answer> Register();
    }
}