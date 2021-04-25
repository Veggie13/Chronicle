using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class User : IPermittable, IEquatable<User>
    {
        public string Name { get; set; }

        public bool IsPermitted(User user)
        {
            return this.Equals(user);
        }

        public override string ToString()
        {
            return $"a({Name})";
        }

        public bool Equals(User other)
        {
            return other != null && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
