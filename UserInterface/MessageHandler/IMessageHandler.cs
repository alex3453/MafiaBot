using System;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface IMessageHandler
    {
        event Action<ICommandInfo> ExCommand;
        event Action<Answer, ulong> SendMassage;
        Task ProcessMessage(SocketMessage msg);
    }
}