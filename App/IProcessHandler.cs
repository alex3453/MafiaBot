using System;
using CommonInteraction;

namespace App
{
    public interface IProcessHandler
    {
        public event Action<User, bool, Answer> SendMassage;
        public void ProcessCommand(CommandInfo ctx);
    }
}