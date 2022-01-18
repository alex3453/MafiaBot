﻿using System;

namespace CommonInteraction
{
    public interface IVisitor<out TCommand>
    {
        TCommand Handle(KillCommandInfo info, Action<Answer, ulong, Service> send);
        TCommand Handle(RegCommandInfo info, Action<Answer, ulong, Service> send);
        TCommand Handle(ResetCommandInfo info, Action<Answer, ulong, Service> send);
        TCommand Handle(StartCommandInfo info, Action<Answer, ulong, Service> send);
        TCommand Handle(VoteCommandInfo info, Action<Answer, ulong, Service> send);
        TCommand Handle(HelpCommandInfo info, Action<Answer, ulong, Service> send);
    }
}