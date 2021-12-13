using System;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface ICommandsHandler
    {
        event Action<CommandInfo> ExCommand;
        event Action<User, bool, Answer> SendMassage;
        Task ProcessMessage(SocketMessage msg);
    }
}