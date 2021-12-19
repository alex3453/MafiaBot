using System;

namespace CommonInteraction
{
    public class HelpCommandInfo : ICommandInfo
    {
        public HelpCommandInfo(User user, bool isComChat, ulong comChatId)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
        }


        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong> send)
        {
            throw new System.NotImplementedException();
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}