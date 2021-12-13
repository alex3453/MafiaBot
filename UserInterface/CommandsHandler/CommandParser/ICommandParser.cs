using System;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface ICommandParser
    {
        CommandInfo Parse(SocketMessage msg);
        string GetCommandsDescription();
    }
}