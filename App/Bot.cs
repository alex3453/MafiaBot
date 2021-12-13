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
        public event Action<User, bool, Answer> SendMassage;

        public Bot(Func<IMafia> createMafiaFunc, ICommandHandler[] commandHandlers, IAnswerTypeHandler[] answerTypeHandlers)
        {
            _createMafiaFunc = createMafiaFunc;
            _commandHandlers = commandHandlers;
            _answerTypeHandlers = answerTypeHandlers;
        }

        private void ReproduceCommand (CommandInfo ctx)
        {
            if (!_usersTeams.Keys.Contains(ctx.User.CommonChannelId))
                _usersTeams[ctx.User.CommonChannelId] = new UsersTeam(_createMafiaFunc);

            foreach (var commandHandler in _commandHandlers)
            {
                if (commandHandler.IsItMyCommand(ctx))
                {
                    foreach (var answerType in commandHandler.ExecuteCommand(_usersTeams[ctx.User.CommonChannelId], ctx))
                    {
                        foreach (var answerTypeHandler in _answerTypeHandlers)
                        {
                            if (answerTypeHandler.IsItMyAnswerType(answerType))
                            {
                                answerTypeHandler.SendMessage(
                                    answerType,
                                    _usersTeams[ctx.User.CommonChannelId],
                                    ctx,
                                    SendMassage);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
}