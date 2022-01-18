using System.Collections.Generic;
using System.Linq;
using CommonInteraction;

namespace UserInterface
{
    public abstract class CommandMessage
    {
        protected abstract ISet<string> PossibleStrings { get; }
        private protected User User;
        private protected bool IsCommonChannel;
        private protected ulong CommonChannelId;
        private protected Service Service;
        public bool IsItMyCommand(MessageData msg)
        {
            var content = msg.Content.Remove(0, 1);
            var com = content.Split().First();
            return PossibleStrings.Contains(com);
        }

        public abstract ICommandInfo GetCommandInfo(MessageData msg);

        protected void FillCommonInfo(MessageData msg)
        {
            Service = msg.Service;
            User = new User(msg.Author.Id, msg.Author.Username);
            IsCommonChannel = msg.IsCommonChannel;
            CommonChannelId = IsCommonChannel ? msg.CommonChannelId : 0;
        }

        public abstract string GetDescription();
    }
}