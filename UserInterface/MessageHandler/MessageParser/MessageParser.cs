using System.Linq;
using System.Text;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class MessageParser : IMessageParser
    {
        private readonly CommandMessage[] _commands;

        public MessageParser(CommandMessage[] commands)
        {
            _commands = commands;
        }
 
        public bool Parse(SocketMessage msg, out CommandInfo commandInfo)
        {
            if (msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!')
            {
                commandInfo = null;
                return false;
            }
            foreach (var command in _commands)
            {
                if (!command.IsItMyCommand(msg)) continue;
                commandInfo = command.CreateCommandInfo(msg);
                return true;
            }
            commandInfo = null;
            return false;
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