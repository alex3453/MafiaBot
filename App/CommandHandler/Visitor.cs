using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor
    {

        public void Handle(KillCommandInfo info, Action<Answer, ulong> send)
        {
            Handler = new KillCommand(info, send);
        }

        public void Handle(RegCommandInfo info, Action<Answer, ulong> send)
        {
            Handler = new RegPlayerCommand(info, send);
        }

        public void Handle(ResetCommandInfo info, Action<Answer, ulong> send)
        {
            Handler = new ResetGameCommand(info, send);
        }

        public void Handle(StartCommandInfo info, Action<Answer, ulong> send)
        {
            Handler =  new StartCommand(info, send);
        }

        public void Handle(VoteCommandInfo info, Action<Answer, ulong> send)
        {
            Handler =  new VoteCommand(info, send);
        }

        public ICommand Handler { get; private set; }
    }
}