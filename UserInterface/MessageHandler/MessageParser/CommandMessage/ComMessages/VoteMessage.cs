using System.Collections.Generic;
using CommonInteraction;

namespace UserInterface
{
    public class VoteMessage : CommandMessage
    {
        protected override ISet<string> PossibleStrings { get; } = new HashSet<string>{"vote", "мщеу"};
        
        public override VoteCommandInfo GetCommandInfo(MessageData msg)
        {
            FillCommonInfo(msg);
            var mentionedUsers = msg.MentionedUsers;
            return new VoteCommandInfo(User, IsCommonChannel, CommonChannelId, mentionedUsers, Service);
        }

        public override string GetDescription() => "!vote {имя игрока на сервере, лучше через @} - " +
                                                   "позволяет голосовать во время самой игры.";
    }
}