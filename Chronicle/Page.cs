using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Chronicle
{
    public class Page
    {
        public string Title { get; set; }
        public IPermittable EditPermission { get; set; }
        public IPermittable ViewPermission { get; set; }
        public IList<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();

        public override string ToString()
        {
            return $"title({Title})\r\n"
                + string.Join("\r\n", ContentBlocks.Select(b => b.ToString()));
        }

        public static Page Parse(string source, IUserStore userStore)
        {
            var whitespacePattern = new Regex(@"^[ \t]*$");
            var blockStartPattern = new Regex(@"^\[\[\[([ \t]*viewers[ \t]*\((.*)\)[ \t]*)?$");
            var blockEndPattern = new Regex(@"^\]\]\]$");

            var page = new Page();

            var lines = source.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int lineNum = 0; lineNum < lines.Length; )
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
