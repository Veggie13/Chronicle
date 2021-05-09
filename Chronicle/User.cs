using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class User : IPermittable, IEquatable<User>
    {
        public Guid ID { get; internal set; }
        public string Name { get; set; }
        public HashedPassword Password { get; set; }

        public bool IsPermitted(User user)
        {
            return this.Equals(user);
        }

        public string Serialized()
        {
            return $"a({ID})";
        }

        public override string ToString()
        {
            return $"a({Name})";
        }

        public bool Equals(User other)
        {
            return other != null
                && ID == other.ID
                && Name == other.Name
                && Password == other.Password;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
