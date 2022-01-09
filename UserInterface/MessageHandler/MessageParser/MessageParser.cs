using System.Linq;
using System.Text;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public class MessageParser
    {
        private readonly CommandMessage[] _botCommands;

        private readonly ViewCommandMessage[] _viewCommands;

        public MessageParser(CommandMessage[] botCommands, ViewCommandMessage[] viewCommands)
        {
            _botCommands = botCommands;
            _viewCommands = viewCommands;
        }
 
        public bool Parse(MessageData msg, out ICommandInfo commandInfo)
        {
            if (msg == null || msg.Author.IsBot || !msg.Content.Any() || msg.Content.First() != '!')
            {
                commandInfo = null;
                return false;
            }
            foreach (var command in _botCommands)
            {
                if (!command.IsItMyCommand(msg)) continue;
                commandInfo = command.GetCommandInfo(msg);
                return true;
            }
            foreach (var command in _viewCommands)
            {
                if (!command.IsItMyCommand(msg)) continue;
                command.ExecuteCommand(msg);
                commandInfo = null;
                return false;
            }
            commandInfo = null;
            return false;
        }

        public string GetCommandsDescription()
        {
            var des = new StringBuilder();
            foreach (var command in _botCommands)
                des.Append(command.GetDescription() + "\n");
            foreach (var command in _viewCommands)
                des.Append(command.GetDescription() + "\n");
            return des.ToString();
        }
    }
}