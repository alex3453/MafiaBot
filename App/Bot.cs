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
        private readonly IDictionary<ulong, GameTeam> _gameTeams = new ConcurrentDictionary<ulong, GameTeam>();
        private readonly Func<IMafia> _createMafiaFunc;

        public Action<ICommandInfo> Register() => ReproduceCommand;
        public event Action<Answer, ulong> SendMassage;
        private IVisitor _visitor;

        public Bot(Func<IMafia> createMafiaFunc)
        {
            _createMafiaFunc = createMafiaFunc;
            _visitor = new Visitor();
        }

        private void ReproduceCommand (ICommandInfo ctx)
        {
            if (ctx.IsComChat && !_gameTeams.Keys.Contains(ctx.ComChatId))
                _gameTeams[ctx.ComChatId] = new GameTeam(ctx.ComChatId, _createMafiaFunc);
            var gameTeam = _gameTeams.Values
                .FirstOrDefault(u => ctx.IsComChat ? u.ChatId == ctx.ComChatId : u.ContainsUser(ctx.User));
            ctx.Accept(_visitor);
            (_visitor.Handler as ICommandHandler)?.ExecuteCommand(gameTeam, SendMassage);
        }
    }
}