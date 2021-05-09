using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronicle
{
    public class PermittableExclusion : IPermittable
    {
        public IList<IPermittable> Items { get; set; } = new List<IPermittable>();

        public PermittableExclusion Excluding(IPermittable item)
        {
            return new PermittableExclusion() { Items = Items.Append(item).ToList() };
        }

        public bool IsPermitted(User user)
        {
            return Items.All(p => !p.IsPermitted(user));
        }

        public string Serialized()
        {
            return $"e({string.Join(',', Items.Select(i => i.Serialized()))})";
        }

        public override string ToString()
        {
            return $"e({string.Join(',', Items.Select(i => i.ToString()))})";
        }
    }
}
