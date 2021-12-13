using System;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface ICommandsHandler
    {
        event Action<User, bool, CommandInfo> ExCommand;
        Task ProcessMessage(SocketMessage msg);
    }
}