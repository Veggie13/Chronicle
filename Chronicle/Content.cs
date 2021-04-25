using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Chronicle
{
    public class Content
    {
        public string Text { get; set; } = "";
        public ISet<string> Properties { get; } = new HashSet<string>();
        public string Link { get; set; } = "";
        public IList<Content> Children { get; set; } = new List<Content>();

        public override string ToString()
        {
            return (Properties.Contains("p") ? "\r\n" : "")
                + (Properties.Contains("b") ? "**" : "")
                + (Properties.Contains("i") ? "*" : "")
                + (!string.IsNullOrEmpty(Link) ? "[[" : "")
                + (!string.IsNullOrEmpty(Link) && Link != Text ? (Link + "|") : "")
                + Text
                + (!string.IsNullOrEmpty(Link) ? "]]" : "")
                + string.Join("", Children.Select(c => c.ToString()))
                + (Properties.Contains("i") ? "*" : "")
                + (Properties.Contains("b") ? "**" : "")
                + (Properties.Contains("p") ? "\r\n" : "");
        }

        static readonly Regex formatPattern = new Regex(@"^(|.*[^\*])((\*){1,3})([^\*].*?)\2(.*)$");
        static readonly Regex linkPattern = new Regex(@"^(.*)\[\[(.*?)(\|(.*?))?\]\](.*)$");

        public static Content Parse(string source)
        {
            var items = fork(source).ToList();
            if (items.Count == 1)
            {
                return items[0];
            }

            return new Content()
            {
                Children = items
            };
        }

        private static IEnumerable<Content> fork(string source)
        {
            if (formatPattern.IsMatch(source))
            {
                var match = formatPattern.Match(source);

                if (!string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    foreach (var leftItem in fork(match.Groups[1].Value))
                    {
                        yield return leftItem;
                    }
                }

                Content item;
                var children = fork(match.Groups[4].Value).ToList();
                if (children.Count > 1)
                {
                    item = new Content()
                    {
                        Children = children
                    };
                }
                else
                {
                    item = children[0];
                }

                switch (match.Groups[2].Value.Length)
                {
                    case 1:
                        item.Properties.Add("i");
                        break;
                    case 2:
                        item.Properties.Add("b");
                        break;
                    case 3:
                        item.Properties.Add("b");
                        item.Properties.Add("i");
                        break;
                    default:
                        throw new Exception();
                }

                yield return item;

                if (!string.IsNullOrEmpty(match.Groups[5].Value))
                {
                    foreach (var rightItem in fork(match.Groups[5].Value))
                    {
                        yield return rightItem;
                    }
                }
            }
            else if (linkPattern.IsMatch(source))
            {
                var match = linkPattern.Match(source);

                if (!string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    foreach (var leftItem in fork(match.Groups[1].Value))
                    {
                        yield return leftItem;
                    }
                }

                var item = new Content()
                {
                    Link = match.Groups[2].Value
                };
                if (string.IsNullOrEmpty(match.Groups[4].Value))
                {
                    item.Text = match.Groups[2].Value;
                }
                else
                {
                    item.Text = match.Groups[4].Value;
                }

                yield return item;

                if (!string.IsNullOrEmpty(match.Groups[5].Value))
                {
                    foreach (var rightItem in fork(match.Groups[5].Value))
                    {
                        yield return rightItem;
                    }
                }
            }
            else
            {
                yield return new Content()
                {
                    Text = source
                };
            }
        }
    }
}
