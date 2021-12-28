using System;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface IMessageHandler
    {
        event Action<ICommandInfo> ExCommand;
        Task ProcessMessage(SocketMessage msg);
    }
}