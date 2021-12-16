using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mafia;
using CommonInteraction;

namespace App
{
    public class Bot
    {
        private readonly IDictionary<ulong, GameTeam> _gameTeams = new ConcurrentDictionary<ulong, GameTeam>();
        private readonly Func<IMafia> _createMafiaFunc;
        private readonly ICommandHandler[] _commandHandlers;

        public Action<CommandInfo> Register() => ReproduceCommand;
        public event Action<Answer, ulong> SendMassage;

        public Bot(ICommandHandler[] commandHandlers, Func<IMafia> createMafiaFunc)
        {
            _commandHandlers = commandHandlers;
            _createMafiaFunc = createMafiaFunc;
        }

        private void ReproduceCommand (CommandInfo ctx)
        {
            if (ctx.IsComChat && !_gameTeams.Keys.Contains(ctx.ComChatId))
                _gameTeams[ctx.ComChatId] = new GameTeam(ctx.ComChatId, _createMafiaFunc);
            var gameTeam = _gameTeams.Values
                .FirstOrDefault(u => ctx.IsComChat ? u.ChatId == ctx.ComChatId : u.ContainsUser(ctx.User));
            _commandHandlers
                .FirstOrDefault(c => c.Type == ctx.CommandType)
                ?.ExecuteCommand(gameTeam, ctx, SendMassage);
        }
    }
}