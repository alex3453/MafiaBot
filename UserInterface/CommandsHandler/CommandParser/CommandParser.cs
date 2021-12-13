using System;
using System.Linq;
using System.Text;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class CommandParser : ICommandParser
    {
        private readonly Command[] _commands;

        public CommandParser(Command[] commands)
        {
            _commands = commands;
        }
 
        public CommandInfo Parse(SocketMessage msg)
        {
            foreach (var command in _commands)
            {
                if (command.IsItMyCommand(msg))
                    return command.CreateCommandInfo(msg);
            }

            return ParseUnknownCommand(msg);
        }

        private CommandInfo ParseUnknownCommand(SocketMessage msg)
        {
            var user = new User(msg.Author.Id, msg.Channel.Id);
            var isCommonChat = msg.Channel.GetType() == typeof(SocketTextChannel);
            return new CommandInfo(user, isCommonChat, CommandType.Unknown);
        }

        public string GetCommandsDescription()
        {
            var des = new StringBuilder();
            foreach (var command in _commands)
            {
                des.Append(command.GetDescription() + "\n");
            }

            return des.ToString();
        }
    }
}