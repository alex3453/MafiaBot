using System;
using System.Threading.Tasks;
using CommonInteraction;

namespace App
{
    public class ProcessHandler : IProcessHandler
    {
        public event Action<User, bool, Answer> SendMassage;

        public void ProcessCommand(CommandInfo ctx)
        {
            var user = ctx.User;
            var isCommonChat = ctx.IsCommonChat;
        }
    }
    
}