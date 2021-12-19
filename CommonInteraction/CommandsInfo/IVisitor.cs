using System;

namespace CommonInteraction
{
    public interface IVisitor
    {
        void Handle(KillCommandInfo info, Action<Answer, ulong> send);
        void Handle(RegCommandInfo info, Action<Answer, ulong> send);
        void Handle(ResetCommandInfo info, Action<Answer, ulong> send);
        void Handle(StartCommandInfo info, Action<Answer, ulong> send);
        void Handle(VoteCommandInfo info, Action<Answer, ulong> send);

        public ICommand Handler { get; }
    }

    public interface ICommand
    {
        
    }
}