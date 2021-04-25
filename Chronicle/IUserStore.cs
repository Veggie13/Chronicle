using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public interface IUserStore
    {
        User GetUser(string name);
    }
}
