using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronicle
{
    public class PermittableIntersection : IPermittable
    {
        public IList<IPermittable> Items { get; set; } = new List<IPermittable>();

        public PermittableIntersection IntersectedWith(IPermittable item)
        {
            return new PermittableIntersection() { Items = Items.Append(item).ToList() };
        }

        public bool IsPermitted(User user)
        {
            return Items.All(p => p.IsPermitted(user));
        }

        public string Serialized()
        {
            return $"i({string.Join(',', Items.Select(i => i.Serialized()))})";
        }

        public override string ToString()
        {
            return $"i({string.Join(',', Items.Select(i => i.ToString()))})";
        }
    }
}
