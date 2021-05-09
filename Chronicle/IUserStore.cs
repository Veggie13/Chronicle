using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public interface IUserStore
    {
        IEnumerable<User> Users { get; }

        bool HasUser(string name);
        User GetUser(Guid id);
        User GetUser(string name);
        void AddUser(User user);
        void UpdateUser(User user);
    }
}
