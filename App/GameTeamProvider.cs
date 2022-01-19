using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class GameTeamProvider
    {
        private readonly Func<IMafia> _createMafiaFunc;
        private readonly IList<GameTeam> _gameTeams = new List<GameTeam>();

        public GameTeamProvider(Func<IMafia> createMafiaFunc)
        {
            _createMafiaFunc = createMafiaFunc;
        }
        public GameTeam GetTeam(ICommandInfo info)
        {
            foreach (var gt in _gameTeams.Where(g => g.Service == info.Service))
            {
                if (info.IsComChat && gt.ChatId == info.ComChatId)
                    return gt;
                if (gt.ContainsUser(info.User))
                    return gt;
            }

            if (!info.IsComChat) return null;
            var team = new GameTeam(info.ComChatId, _createMafiaFunc, info.Service);
            _gameTeams.Add(team);
            return team;
        }
    }
}