using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor<ICommandHandler>
    {
        public ICommandHandler Handle(KillCommandInfo info, Action<Answer, ulong> send) 
            => new KillCommand(info, send);
        public ICommandHandler Handle(RegCommandInfo info, Action<Answer, ulong> send) 
            => new RegPlayerCommand(info, send);
        public ICommandHandler Handle(ResetCommandInfo info, Action<Answer, ulong> send) 
            => new ResetGameCommand(info, send);
        public ICommandHandler Handle(StartCommandInfo info, Action<Answer, ulong> send) 
            => new StartCommand(info, send);
        public ICommandHandler Handle(VoteCommandInfo info, Action<Answer, ulong> send)
            => new VoteCommand(info, send);

        public ICommandHandler Handle(HelpCommandInfo info, Action<Answer, ulong> send)
            => new HelpCommand(info, send);
    }
}