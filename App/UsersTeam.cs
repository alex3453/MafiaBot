using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class UsersTeam
    {
        public ulong ChatId { get; }
        public IMafia Mafia { get; private set; }
        
        private ISet<User> _usersInGame = new HashSet<User>();
        public IReadOnlySet<User> Users => (IReadOnlySet<User>) _usersInGame;
        
        private readonly Func<IMafia> _createMafiaFunc;

        public UsersTeam(ulong chatId, Func<IMafia> createMafiaFunc)
        {
            _createMafiaFunc = createMafiaFunc;
            ChatId = chatId;
            Mafia = _createMafiaFunc();
        }

        public void AddUser(User user) => _usersInGame.Add(user);
        public bool ContainsUser(User user) => _usersInGame.Contains(user);

        public void Reset()
        {
            Mafia = _createMafiaFunc();
            _usersInGame = new HashSet<User>();
        }
    }
}