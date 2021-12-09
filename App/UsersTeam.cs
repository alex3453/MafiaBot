using System;
using System.Collections.Generic;
using System.Linq;
using CommonInteraction;
using Mafia;

namespace App
{
    public class UsersTeam
    {
        public readonly User CommonChat;
        private readonly IList<User> users = new List<User>();
        public IMafia Mafia { get; private set; }
        public IReadOnlyList<User> Users => (IReadOnlyList<User>) users;

        public UsersTeam(User commonChat) => CommonChat = commonChat;

        public void AddUser(User user) => users.Add(user);

        public void DeleteUserByName(string name, Action<User> deleteUser)
        {
            var user = GetUserByName(name);
            users.Remove(user);
            deleteUser(user);
        }

        public User GetUserByName(string name) => users.First(u => u.Name == name);

        public void SetMafia(IMafia mafia) => Mafia = mafia;
    }
}