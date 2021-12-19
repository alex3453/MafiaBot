using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public class VoteCommandInfo : ICommandInfo
    {
        public readonly IEnumerable<string> MentPlayers;

        public VoteCommandInfo(User user, bool isComChat, ulong comChatId, IEnumerable<string> mentPlayers)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            MentPlayers = mentPlayers;
        }

        public THandler Accept<THandler>(IVisitor<THandler> visitor, Action<Answer, ulong> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}