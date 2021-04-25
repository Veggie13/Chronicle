using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class User : IPermittable
    {
        public string Name { get; set; }

        public bool IsPermitted(User user)
        {
            return user == this;
        }

        public static bool operator ==(User a, User b)
        {
            return a.Name == b.Name;
        }

        public static bool operator !=(User a, User b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"a({Name})";
        }
    }
}
