﻿using System;
using CommonInteraction;

namespace App.CommandHandler
{
    public class Visitor : IVisitor<BaseCommandHandler>
    {
        public BaseCommandHandler Handle(KillCommandInfo info, Action<Answer, ulong, Service> send) 
            => new KillBaseCommand(info, send);
        public BaseCommandHandler Handle(RegCommandInfo info, Action<Answer, ulong, Service> send) 
            => new RegPlayerBaseCommand(info, send);
        public BaseCommandHandler Handle(ResetCommandInfo info, Action<Answer, ulong, Service> send) 
            => new ResetGameBaseCommand(info, send);
        public BaseCommandHandler Handle(StartCommandInfo info, Action<Answer, ulong, Service> send) 
            => new StartBaseCommand(info, send);
        public BaseCommandHandler Handle(VoteCommandInfo info, Action<Answer, ulong, Service> send)
            => new VoteBaseCommand(info, send);

        public BaseCommandHandler Handle(HelpCommandInfo info, Action<Answer, ulong, Service> send)
            => new HelpBaseCommand(info, send);
    }
}