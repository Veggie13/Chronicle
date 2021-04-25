using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Chronicle
{
    public class FilePageStore : IPageStore
    {
        private IPageStore _backingStore;

        public FilePageStore(FileApplicationContext context, IUserStore userStore)
            : this(context, userStore, new PageStore())
        { }

        public FilePageStore(FileApplicationContext context, IUserStore userStore, IPageStore backingStore)
        {
            _backingStore = backingStore;
            Context = context;

            string[] authorsLines;
            using (var reader = new StreamReader(Context.AuthorsFile.FullName))
            {
                authorsLines = reader.ReadToEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            var authorPattern = new Regex(@"^(.*)\|(.*)$");
            var authors = authorsLines.ToDictionary(l => authorPattern.Match(l).Groups[1].Value, l => authorPattern.Match(l).Groups[2].Value);

            foreach (var pageInfo in Context.PageFolder.GetFiles("*.txt"))
            {
                string content;
                using (var reader = new StreamReader(pageInfo.FullName))
                {
                    content = reader.ReadToEnd();
                }

                var page = Page.Parse(content, userStore);
                page.Title = Path.GetFileNameWithoutExtension(pageInfo.Name);
                page.Author = userStore.GetUser(authors[page.Title]);
                _backingStore.AddPage(page);
            }
        }

        public FileApplicationContext Context { get; private set; }

        public IEnumerable<Page> Pages => _backingStore.Pages;

        public void AddPage(Page page)
        {
            _backingStore.AddPage(page);

            using (var writer = new StreamWriter(Path.Combine(Context.PageFolder.FullName, $"{page.Title}.txt")))
            {
                writer.Write(page);
            }

            updateAuthors();
        }

        public Page GetPage(string title)
        {
            return _backingStore.GetPage(title);
        }

        public bool PageExists(string title)
        {
            return _backingStore.PageExists(title);
        }

        private void updateAuthors()
        {
            using (var writer = new StreamWriter(Context.AuthorsFile.FullName))
            {
                foreach (var page in Pages)
                {
                    writer.WriteLine($"{page.Title}|{page.Author.Name}");
                }
            }
        }
    }
}
