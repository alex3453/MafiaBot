using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Discord.WebSocket;

namespace UserInterface
{
    public abstract class CommandMessage
    {
        protected abstract ISet<string> PossibleStrings { get; }
        private protected User user;
        private protected bool isCommonChannel;
        private protected ulong commonChannelId;
        private protected string[] mentionedUsers;
        private protected string[] args;
        public bool IsItMyCommand(SocketMessage msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }
        
        public void PrepareCommandInfo(SocketMessage msg)
        {
            mentionedUsers = msg.MentionedUsers.Select(x => x.Username).ToArray();
            args = msg.Content.Split().Skip(1).ToArray();
            user = new User(msg.Author.Id, msg.Author.Username);
            isCommonChannel = msg.Channel.GetType() == typeof(SocketTextChannel);
            commonChannelId = 0;
            if (isCommonChannel)
                commonChannelId = msg.Channel.Id;
            // return new CommandInfo(
            //     user, 
            //     MyCommandType, 
            //     isCommonChannel, 
            //     commonChannelId, 
            //     mentionedUsers, 
            //     args);
        }

        public abstract ICommandInfo GetCommandInfo();

        public abstract string GetDescription();
    }
}