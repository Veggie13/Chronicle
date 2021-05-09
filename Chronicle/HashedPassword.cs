using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class HashedPassword : IEquatable<string>
    {
        private HashedPassword() { }

        public static HashedPassword Create(string rawPassword)
        {
            return new HashedPassword() { Hash64 = PasswordHasher.Hash(rawPassword) };
        }

        public static HashedPassword FromHash(string hashedPassword)
        {
            return new HashedPassword() { Hash64 = hashedPassword };
        }

        public string Hash64 { get; private set; }

        public bool Equals(string rawPassword)
        {
            return PasswordHasher.Verify(rawPassword, Hash64);
        }

        public override bool Equals(object obj)
        {
            if (obj is string) return Equals(obj as string);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Hash64.GetHashCode();
        }

        public override string ToString()
        {
            return $"HashedPassword{{{Hash64}}}";
        }
    }
}
