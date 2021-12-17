using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor
    {

        public void Handle(KillCommandInfo info)
        {
            Handler = new KillCommand(info);
        }

        public void Handle(RegCommandInfo info)
        {
            Handler = new RegPlayerCommand(info);
        }

        public void Handle(ResetCommandInfo info)
        {
            Handler = new ResetGameCommand(info);
        }

        public void Handle(StartCommandInfo info)
        {
            Handler =  new StartCommand(info);
        }

        public void Handle(VoteCommandInfo info)
        {
            Handler =  new VoteCommand(info);
        }

        public ICommand Handler { get; private set; }
    }
}