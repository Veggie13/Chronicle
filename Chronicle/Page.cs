using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Chronicle
{
    public class Page
    {
        public string Title { get; set; }
        public User Author { get; set; }
        public IPermittable EditPermission { get; set; } = Permittable.None;
        public IPermittable ViewPermission { get; set; } = Permittable.All;
        public IList<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();

        public override string ToString()
        {
            return $"title({Title})\r\n"
                + $"editors({EditPermission})\r\n"
                + $"viewers({ViewPermission})\r\n"
                + string.Join("\r\n", ContentBlocks.Select(b => b.ToString()));
        }

        public static Page Parse(string source, IUserStore userStore)
        {
            var whitespacePattern = new Regex(@"^[ \t]*$");
            var titleBlockPattern = new Regex(@"^[ \t]*(title|editors|viewers)[ \t]*\((.*)\)[ \t]*$");
            var blockStartPattern = new Regex(@"^\[\[\[([ \t]*viewers[ \t]*\((.*)\)[ \t]*)?$");
            var blockEndPattern = new Regex(@"^\]\]\]$");

            var page = new Page();

            var lines = source.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = 0;
            for (; lineNum < lines.Length; lineNum++)
            {
                string line = lines[lineNum];

                if (whitespacePattern.IsMatch(line))
                {
                    continue;
                }

                if (!titleBlockPattern.IsMatch(line))
                {
                    break;
                }

                var match = titleBlockPattern.Match(line);
                switch (match.Groups[1].Value)
                {
                    case "title":
                        page.Title = match.Groups[2].Value;
                        break;
                    case "editors":
                        page.EditPermission = Permittable.Parse(match.Groups[2].Value, userStore);
                        break;
                    case "viewers":
                        page.ViewPermission = Permittable.Parse(match.Groups[2].Value, userStore);
                        break;
                }
            }
            
            for (; lineNum < lines.Length; )
            {
                string line = lines[lineNum];

                if (whitespacePattern.IsMatch(line))
                {
                    lineNum++;
                    continue;
                }

                var block = new ContentBlock();
                
                bool explicitBlock = blockStartPattern.IsMatch(line);
                if (explicitBlock)
                {
                    var match = blockStartPattern.Match(line);
                    if (match.Groups[1].Length > 0)
                    {
                        block.ViewPermission = Permittable.Parse(match.Groups[1].Value, userStore);
                    }
                    lineNum++;
                }

                var contentItems = new List<Content>();

                for (; lineNum < lines.Length; lineNum++)
                {
                    line = lines[lineNum];

                    if (explicitBlock && blockEndPattern.IsMatch(line))
                    {
                        lineNum++;
                        break;
                    }
                    else if (!explicitBlock && blockStartPattern.IsMatch(line))
                    {
                        break;
                    }

                    var contentItem = Content.Parse(line);
                    contentItem.Properties.Add("p");
                    contentItems.Add(contentItem);
                }

                if (contentItems.Count == 1)
                {
                    block.Content = contentItems[0];
                }
                else
                {
                    block.Content = new Content()
                    {
                        Children = contentItems
                    };
                }

                page.ContentBlocks.Add(block);
            }

            return page;
        }
    }
}
