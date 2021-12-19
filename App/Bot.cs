using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.CommandHandler;
using Mafia;
using CommonInteraction;

namespace App
{
    public class Bot
    {
        public Action<ICommandInfo> Register() => ReproduceCommand;
        public event Action<Answer, ulong> SendMassage;
        private readonly IVisitor _visitor;
        private readonly IDictionaryProvider _teamProvider;

        public Bot(IVisitor visitor, IDictionaryProvider teamProvider )
        {
            _visitor = visitor;
            _teamProvider = teamProvider;
        }

        private void ReproduceCommand (ICommandInfo ctx)
        {
            var gameTeam = _teamProvider.GetTeam(ctx);
            ctx.Accept(_visitor, SendMassage);
            (_visitor.Handler as ICommandHandler)?.ExecuteCommand(gameTeam);
        }
    }
}