using System;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public interface IMessageParser
    {
        bool Parse(MessageData msg, out ICommandInfo commandInfo);
        string GetCommandsDescription();
    }
}