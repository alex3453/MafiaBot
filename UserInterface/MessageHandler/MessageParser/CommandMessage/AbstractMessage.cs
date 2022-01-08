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
        public bool IsItMyCommand(MessageData msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }

        public abstract ICommandInfo GetCommandInfo(MessageData msg);

        protected void FillCommonInfo(MessageData msg)
        {
            user = new User(msg.Author.Id, msg.Author.Username);
            isCommonChannel = msg.IsCommonChannel;
            commonChannelId = isCommonChannel ? msg.CommonChannelId : 0;
        }

        public abstract string GetDescription();
    }
}