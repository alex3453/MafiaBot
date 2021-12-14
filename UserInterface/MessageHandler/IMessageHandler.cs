using System;
using System.Threading.Tasks;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface IMessageHandler
    {
        event Action<CommandInfo> ExCommand;
        event Action<bool, Answer, ulong> SendMassage;
        Task ProcessMessage(SocketMessage msg);
    }
}