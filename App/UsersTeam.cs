using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class UsersTeam
    {
        private ISet<User> users = new HashSet<User>();
        public IMafia Mafia { get; private set; }
        public bool IsMafiaSetted { get; private set; }
        public IReadOnlySet<User> Users => (IReadOnlySet<User>) users;

        public void AddUser(User user) => users.Add(user);
        public bool IsContainsUser(User user) => users.Contains(user);

        public void SetMafia()
        {
            Mafia = new MafiaGame(new SimpleRoleDist());
            IsMafiaSetted = true;
        }
    }
}