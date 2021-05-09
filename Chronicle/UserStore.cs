using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class UserStore : IUserStore
    {
        private Dictionary<string, User> _users = new Dictionary<string, User>();

        public IEnumerable<User> Users
        {
            get { return _users.Values; }
        }

        public void AddUser(User user)
        {
            _users[user.Name] = user;
        }

        public bool HasUser(string name)
        {
            return _users.ContainsKey(name);
        }

        public User GetUser(string name)
        {
            return _users[name];
        }
    }
}
