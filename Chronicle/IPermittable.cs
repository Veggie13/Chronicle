using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chronicle
{
    public interface IPermittable
    {
        bool IsPermitted(User user);
        string Serialized();
    }

    public static class Permittable
    {
        public static IPermittable None { get; } = new PermittableIntersection();
        public static IPermittable All { get; } = new PermittableExclusion();

        public static PermittableUnion United(this IEnumerable<IPermittable> @this)
        {
            return new PermittableUnion() { Items = @this.ToList() };
        }

        public static PermittableUnion UnitedWith(this IPermittable @this, IPermittable item)
        {
            return new PermittableUnion() { Items = { @this, item } };
        }

        public static PermittableIntersection Intersected(this IEnumerable<IPermittable> @this)
        {
            return new PermittableIntersection() { Items = @this.ToList() };
        }

        public static PermittableIntersection IntersectedWith(this IPermittable @this, IPermittable item)
        {
            return new PermittableIntersection() { Items = { @this, item } };
        }

        public static PermittableExclusion Excluded(this IEnumerable<IPermittable> @this)
        {
            return new PermittableExclusion() { Items = @this.ToList() };
        }

        public static PermittableExclusion Excluding(this IPermittable @this, IPermittable item)
        {
            return new PermittableExclusion() { Items = { @this, item } };
        }

        public static IPermittable Parse(string permitString, IUserStore userStore, Func<IUserStore, string, User> userParseStrategy)
        {
            var itemPattern = new Regex(@"a *\( *([A-Za-z0-9_\-\{\}]+) *\)");
            var groupPattern = new Regex(@"([uie]) *\( *p([0-9]+) *(\, *p([0-9]+) *)*\)");

            var items = new List<IPermittable>();
            while (itemPattern.IsMatch(permitString))
            {
                var match = itemPattern.Match(permitString);
                string itemStr = match.Groups[1].Value;
                var user = userParseStrategy(userStore, itemStr);
                permitString = itemPattern.Replace(permitString, $"p{items.Count}", 1);
                items.Add(user);
            }
            while (groupPattern.IsMatch(permitString))
            {
                var match = groupPattern.Match(permitString);
                IEnumerable<IPermittable> groupItems = new[] { items[int.Parse(match.Groups[2].Value)] };
                groupItems = groupItems.Concat(match.Groups[4].Captures.Select(c => items[int.Parse(c.Value)]));
                permitString = groupPattern.Replace(permitString, $"p{items.Count}", 1);
                switch (match.Groups[1].Value)
                {
                    case "u":
                        items.Add(groupItems.United());
                        break;
                    case "i":
                        items.Add(groupItems.Intersected());
                        break;
                    case "e":
                        items.Add(groupItems.Excluded());
                        break;
                    default:
                        continue;
                }
            }

            return items[items.Count - 1];
        }

        public static IPermittable Parse(string permitString, IUserStore userStore)
        {
            return Parse(permitString, userStore, (us, str) => us.GetUser(str));
        }
    }
}
