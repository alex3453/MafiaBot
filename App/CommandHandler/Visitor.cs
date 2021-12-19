using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor<ICommandHandler>
    {

        public ICommandHandler Handle(KillCommandInfo info, Action<Answer, ulong> send)
        {
            return new KillCommand(info, send);
        }

        public ICommandHandler Handle(RegCommandInfo info, Action<Answer, ulong> send)
        {
            return new RegPlayerCommand(info, send);
        }

        public ICommandHandler Handle(ResetCommandInfo info, Action<Answer, ulong> send)
        {
            return new ResetGameCommand(info, send);
        }

        public ICommandHandler Handle(StartCommandInfo info, Action<Answer, ulong> send)
        {
            return new StartCommand(info, send);
        }

        public ICommandHandler Handle(VoteCommandInfo info, Action<Answer, ulong> send)
        {
            return new VoteCommand(info, send);
        }
    }
}