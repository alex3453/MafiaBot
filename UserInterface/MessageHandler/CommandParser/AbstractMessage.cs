using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public abstract class CommandMessage
    {
        protected abstract ISet<string> PossibleStrings { get; }
        protected abstract CommandType MyCommandType { get; }
        
        public bool IsItMyCommand(SocketMessage msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }
        
        public CommandInfo CreateCommandInfo(SocketMessage msg)
        {
            var mentionedUsers = msg.MentionedUsers.Select(x => x.Username).ToArray();
            var args = msg.Content.Split().Skip(1).ToArray();
            var user = new User(msg.Author.Id, msg.Channel.Id, msg.Author.Username);
            var isCommonChat = msg.Channel.GetType() == typeof(SocketTextChannel);
            return new CommandInfo(user, isCommonChat, MyCommandType, mentionedUsers, args);
        }

        public abstract string GetDescription();
    }
}