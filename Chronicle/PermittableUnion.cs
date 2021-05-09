using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronicle
{
    public class PermittableUnion : IPermittable
    {
        public IList<IPermittable> Items { get; set; } = new List<IPermittable>();

        public PermittableUnion UnitedWith(IPermittable item)
        {
            return new PermittableUnion() { Items = Items.Append(item).ToList() };
        }

        public bool IsPermitted(User user)
        {
            return Items.Any(p => p.IsPermitted(user));
        }

        public string Serialized()
        {
            return $"u({string.Join(',', Items.Select(i => i.Serialized()))})";
        }

        public override string ToString()
        {
            return $"u({string.Join(',', Items.Select(i => i.ToString()))})";
        }
    }
}
