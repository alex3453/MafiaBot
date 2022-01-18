using System;
using CommonInteraction;

namespace App
{
    public class Bot
    {
        public Action<ICommandInfo> Register() => ReproduceCommand;
        public event Action<Answer, ulong, string> SendMassage;
        private readonly IVisitor<BaseCommandHandler> _visitor;
        private readonly GameTeamProvider _teamProvider;

        public Bot(IVisitor<BaseCommandHandler> visitor, GameTeamProvider teamProvider )
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