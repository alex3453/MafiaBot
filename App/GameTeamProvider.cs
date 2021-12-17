using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class GameTeamProvider : IDictionaryProvider
    {
        public IDictionary<ulong, GameTeam> GameTeams => _gameTeams;
        private readonly Func<IMafia> _createMafiaFunc;
        private readonly IDictionary<ulong, GameTeam> _gameTeams = new Dictionary<ulong, GameTeam>();

        public GameTeamProvider(Func<IMafia> createMafiaFunc)
        {
            _createMafiaFunc = createMafiaFunc;
        }
        public GameTeam GetTeam(ICommandInfo info)
        {
            if (info.IsComChat && !_gameTeams.Keys.Contains(info.ComChatId))
                _gameTeams[info.ComChatId] = new GameTeam(info.ComChatId, _createMafiaFunc);
            return _gameTeams.Values
                .FirstOrDefault(u => info.IsComChat ? u.ChatId == info.ComChatId : u.ContainsUser(info.User));
        }
    }
}