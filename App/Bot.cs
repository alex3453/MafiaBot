using System;
using CommonInteraction;

namespace App
{
    public class Bot
    {
        public Action<ICommandInfo> Register() => ReproduceCommand;
        public event Action<Answer, ulong> SendMassage;
        private readonly IVisitor<ICommandHandler> _visitor;
        private readonly IDictionaryProvider _teamProvider;

        public Bot(IVisitor<ICommandHandler> visitor, IDictionaryProvider teamProvider )
        {
            _visitor = visitor;
            _teamProvider = teamProvider;
        }

        private void ReproduceCommand (ICommandInfo ctx)
        {
            var gameTeam = _teamProvider.GetTeam(ctx);
            ctx.Accept(_visitor, SendMassage).ExecuteCommand(gameTeam);
        }
    }
}