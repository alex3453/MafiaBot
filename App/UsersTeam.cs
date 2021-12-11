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
        public void DeleteUserByName(string name, Action<ulong> deleteUser)
        {
            var user = GetUserByName(name);
            users.Remove(user);
            deleteUser(user.Id);
        }

        public void LeaveOneUser(Action<ulong> deleteUser, User user)
        {
            foreach (var usr in users)
            {
                if (usr != user)
                    deleteUser(user.Id);
            }
            users = new HashSet<User>();
            users.Add(user);
        }

        public User GetUserByName(string name) => users.First(u => u.Name == name);

        public void SetMafia()
        {
            Mafia = new MafiaGame(new SimpleRoleDist());
            IsMafiaSetted = true;
        }
    }
}