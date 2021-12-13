using System;
using System.Threading.Tasks;
using CommonInteraction;

namespace App
{
    public interface IProcessHandler
    {
        public event Action<User, bool, Answer> SendMassage;
        public Task ProcessCommand(CommandInfo ctx);
    }
}