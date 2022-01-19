using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor<AbstractCommandHandler>
    {
        public AbstractCommandHandler Handle(KillCommandInfo info, Action<Answer, ulong, string> send) 
            => new KillComHandler(info, send);
        public AbstractCommandHandler Handle(RegCommandInfo info, Action<Answer, ulong, string> send) 
            => new RegComHandler(info, send);
        public AbstractCommandHandler Handle(ResetCommandInfo info, Action<Answer, ulong, string> send) 
            => new ResetComHandler(info, send);
        public AbstractCommandHandler Handle(StartCommandInfo info, Action<Answer, ulong, string> send) 
            => new StartComHandler(info, send);
        public AbstractCommandHandler Handle(VoteCommandInfo info, Action<Answer, ulong, string> send)
            => new VoteComHandler(info, send);
    }
}