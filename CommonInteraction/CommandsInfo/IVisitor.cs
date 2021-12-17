using CommonInteraction;

namespace App.CommandHandler
{
    public interface IVisitor
    {
        void Handle(KillCommandInfo info);
        void Handle(RegCommandInfo info);
        void Handle(ResetCommandInfo info);
        void Handle(StartCommandInfo info);
        void Handle(VoteCommandInfo info);

        public ICommand Handler { get; }
    }

    public interface ICommand
    {
        
    }
}