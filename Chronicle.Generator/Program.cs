using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chronicle;

namespace Chronicle.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectPath = @"C:\Users\Veggie\Google Drive\D&D\Dragonseye\wiki";

            var fileAppCtx = new FileApplicationContext(projectPath);

            var userLines = File.ReadAllLines(fileAppCtx.UserListFile.FullName);
            var users = userLines.Select(l => l.Split('\t')).ToDictionary(l => l[0], l => new { ID = Guid.NewGuid(), Password = l[1] });
            using (var writer = new StreamWriter(fileAppCtx.UserListFile.FullName))
            {
                foreach (var userName in users.Keys)
                {
                    writer.WriteLine($"{users[userName].ID}\t{userName}\t{PasswordHasher.Hash(users[userName].Password)}");
                }
            }

            var pageFiles = fileAppCtx.PageFolder.GetFiles("*.txt");
            var pages = pageFiles.ToDictionary(pf => Path.GetFileNameWithoutExtension(pf.Name), pf => Guid.NewGuid());
            using (var writer = new StreamWriter(fileAppCtx.IndexFile.FullName))
            {
                foreach (var title in pages.Keys)
                {
                    writer.WriteLine($"{pages[title]}\t{title}\t{title}.txt");
                }
            }

            var linkPattern = new Regex(@"\[\[(.+?)(\|.+?)?\]\]");
            foreach (var file in pageFiles)
            {
                string content = File.ReadAllText(file.FullName);
                content = linkPattern.Replace(content, m => $"[[{pages[m.Groups[1].Value]}{m.Groups[2].Value}]]");
                File.WriteAllText(file.FullName, content);
            }

            var authors = new Dictionary<Guid, Guid>();
            if (fileAppCtx.AuthorsFile.Exists)
            {
                var authorLines = File.ReadAllLines(fileAppCtx.AuthorsFile.FullName);
                authors = authorLines.Select(l => l.Split('\t')).ToDictionary(l => pages[l[0]], l => users[l[1]].ID);
            }
            using (var writer = new StreamWriter(fileAppCtx.AuthorsFile.FullName))
            {
                foreach (var pageID in pages.Values)
                {
                    if (authors.ContainsKey(pageID))
                    {
                        writer.WriteLine($"{pageID}\t{authors[pageID]}");
                    }
                    else
                    {
                        writer.WriteLine($"{pageID}\t{users.First().Value.ID}");
                    }
                }
            }
        }
    }
}
