﻿using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public class VoteCommandInfo : ICommandInfo
    {
        public readonly IEnumerable<string> MentPlayers;

        public VoteCommandInfo(User user, bool isComChat, ulong comChatId, IEnumerable<string> mentPlayers, string service)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            MentPlayers = mentPlayers;
            Service = service;
        }

        public string Service { get; }

        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong, string> send)
        {
            return visitor.Handle(this, send);
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}