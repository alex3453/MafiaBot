using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class UsersTeam
    {
        private ISet<User> _usersInGame = new HashSet<User>();
        public IMafia Mafia { get; private set; }
        private readonly Func<IMafia> _createMafiaFunc;

        public UsersTeam(Func<IMafia> createMafiaFunc)
        {
            _createMafiaFunc = createMafiaFunc;
            Mafia = _createMafiaFunc();
        }
        public IReadOnlySet<User> Users => (IReadOnlySet<User>) _usersInGame;

        public void AddUser(User user) => _usersInGame.Add(user);
        public bool ContainsUser(User user) => _usersInGame.Contains(user);

        public void ResetMafia()
        {
            Mafia = _createMafiaFunc();
            _usersInGame = new HashSet<User>();
        }
    }
}