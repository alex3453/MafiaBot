using System;

namespace CommonInteraction
{
    public interface IVisitor<out THandler>
    {
        THandler Handle(KillCommandInfo info, Action<Answer, ulong> send);
        THandler Handle(RegCommandInfo info, Action<Answer, ulong> send);
        THandler Handle(ResetCommandInfo info, Action<Answer, ulong> send);
        THandler Handle(StartCommandInfo info, Action<Answer, ulong> send);
        THandler Handle(VoteCommandInfo info, Action<Answer, ulong> send);
    }
}