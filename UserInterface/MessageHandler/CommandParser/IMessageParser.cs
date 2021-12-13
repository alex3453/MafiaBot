using System;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface IMessageParser
    {
        CommandInfo Parse(SocketMessage msg);
        string GetCommandsDescription();
    }
}