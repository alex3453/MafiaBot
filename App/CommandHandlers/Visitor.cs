using System;
using App.CommandHandler;
using CommonInteraction;

namespace App
{
    public class Visitor : IVisitor<BaseCommandHandler>
    {
        public BaseCommandHandler Handle(KillCommandInfo info, Action<Answer, ulong> send) 
            => new KillCommand(info, send);
        public BaseCommandHandler Handle(RegCommandInfo info, Action<Answer, ulong> send) 
            => new RegPlayerCommand(info, send);
        public BaseCommandHandler Handle(ResetCommandInfo info, Action<Answer, ulong> send) 
            => new ResetGameCommand(info, send);
        public BaseCommandHandler Handle(StartCommandInfo info, Action<Answer, ulong> send) 
            => new StartCommand(info, send);
        public BaseCommandHandler Handle(VoteCommandInfo info, Action<Answer, ulong> send)
            => new VoteCommand(info, send);

        public BaseCommandHandler Handle(HelpCommandInfo info, Action<Answer, ulong> send)
            => new HelpCommand(info, send);
    }
}