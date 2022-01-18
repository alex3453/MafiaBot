using System;
using System.Collections.Generic;


namespace CommonInteraction
{
    public class KillCommandInfo : ICommandInfo
    {
        public readonly IEnumerable<string> Content;

        public Service Service { get; }

        public TCommand Accept<TCommand>(IVisitor<TCommand> visitor, Action<Answer, ulong, Service> send)
        {
            return visitor.Handle(this, send);
        }
        public KillCommandInfo(User user, bool isComChat, ulong comChatId, IEnumerable<string> content, Service service)
        {
            User = user;
            IsComChat = isComChat;
            ComChatId = comChatId;
            Content = content;
            Service = service;
        }

        public User User { get; }
        public bool IsComChat { get; }
        public ulong ComChatId { get; }
    }
}