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
        private readonly IDictionary<ulong, UsersTeam> _usersTeams = new ConcurrentDictionary<ulong, UsersTeam>();
        private readonly Func<IMafia> _createMafiaFunc;
        private readonly ICommandHandler[] _commandHandlers;
        private readonly IAnswerTypeHandler[] _answerTypeHandlers;
        
        public Action<CommandInfo> Register() => ReproduceCommand;
        public event Action<bool, Answer, ulong> SendMassage;

        public Bot(ICommandHandler[] commandHandlers, IAnswerTypeHandler[] answerTypeHandlers, Func<IMafia> createMafiaFunc)
        {
            _commandHandlers = commandHandlers;
            _answerTypeHandlers = answerTypeHandlers;
            _createMafiaFunc = createMafiaFunc;
        }

        private void ReproduceCommand (CommandInfo ctx)
        {
            if (ctx.IsComChat && !_usersTeams.Keys.Contains(ctx.ComChatId))
                _usersTeams[ctx.User.CommonChannelId] = new UsersTeam(ctx.User.CommonChannelId);
            var comHandler = _commandHandlers.First(c => c.Type == ctx.CommandType);
            var team = !ctx.IsComChat ? _usersTeams.Values.First(t => t.ContainsUser(ctx.User)) : _usersTeams[ctx.User.CommonChannelId];
            foreach (var answerType in comHandler.ExecuteCommand(team, ctx))
                _answerTypeHandlers
                    .First(a => a.IsItMyAnswerType(answerType))
                    .SendMessage(answerType, team, ctx, SendMassage);
            
        }
    }
}