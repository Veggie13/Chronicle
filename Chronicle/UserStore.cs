using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class UserStore : IUserStore
    {
        private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();
        private Dictionary<string, Guid> _usersByName = new Dictionary<string, Guid>();
        private Dictionary<Guid, string> _namesByUser = new Dictionary<Guid, string>();

        public IEnumerable<User> Users
        {
            get { return _users.Values; }
        }

        public void AddUser(User user)
        {
            if (_usersByName.ContainsKey(user.Name))
            {
                throw new Exception("Duplicate user name.");
            }
            if (user.ID == Guid.Empty)
            {
                user.ID = Guid.NewGuid();
            }
            _users[user.ID] = user;
            _usersByName[user.Name] = user.ID;
            _namesByUser[user.ID] = user.Name;
        }

        public bool HasUser(string name)
        {
            return _usersByName.ContainsKey(name);
        }

        public User GetUser(string name)
        {
            return _users[_usersByName[name]];
        }

        public User GetUser(Guid id)
        {
            return _users[id];
        }

        public void UpdateUser(User user)
        {
            if (_namesByUser[user.ID] != user.Name)
            {
                if (_usersByName.ContainsKey(user.Name))
                {
                    throw new Exception("Duplicate Name in user.");
                }
                _usersByName.Remove(_namesByUser[user.ID]);
                _usersByName[user.Name] = user.ID;
                _namesByUser[user.ID] = user.Name;
            }
        }
    }
}
