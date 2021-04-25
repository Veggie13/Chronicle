using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class UserStore : IUserStore
    {
        private Dictionary<string, User> _users = new Dictionary<string, User>();

        public void AddUser(User user)
        {
            _users[user.Name] = user;
        }

        public User GetUser(string name)
        {
            return _users[name];
        }
    }
}
