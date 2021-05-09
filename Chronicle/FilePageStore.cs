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
        private Dictionary<Guid, FileInfo> _pageFiles = new Dictionary<Guid, FileInfo>();

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
            var authorPattern = new Regex(@"^(.*)\t(.*)$");
            var authors = authorsLines.ToDictionary(
                l => new Guid(authorPattern.Match(l).Groups[1].Value),
                l => new Guid(authorPattern.Match(l).Groups[2].Value));

            string[] indexLines;
            using (var reader = new StreamReader(Context.IndexFile.FullName))
            {
                indexLines = reader.ReadToEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            var indexPattern = new Regex(@"^(.*)\t(.*)\t(.*)$");
            var titles = indexLines.ToDictionary(
                l => new Guid(indexPattern.Match(l).Groups[1].Value),
                l => indexPattern.Match(l).Groups[2].Value);
            var fileNames = indexLines.ToDictionary(
                l => new Guid(indexPattern.Match(l).Groups[1].Value),
                l => indexPattern.Match(l).Groups[3].Value);

            foreach (var id in titles.Keys)
            {
                _backingStore.AddPage(new Page()
                {
                    ID = id,
                    Title = titles[id],
                    Author = userStore.GetUser(authors[id])
                });
            }

            foreach (var page in Pages)
            {
                var pageInfo = Context.PageFolder.GetFiles(fileNames[page.ID])[0];
                _pageFiles[page.ID] = pageInfo;

                string content;
                using (var reader = new StreamReader(pageInfo.FullName))
                {
                    content = reader.ReadToEnd();
                }

                page.Deserialize(content, userStore, _backingStore);
            }
        }

        public FileApplicationContext Context { get; private set; }

        public IEnumerable<Page> Pages => _backingStore.Pages;

        public void AddPage(Page page)
        {
            _backingStore.AddPage(page);
            using (var writer = new StreamWriter(Context.IndexFile.FullName, true))
            {
                writer.WriteLine();
                writer.WriteLine($"{page.ID}\t{page.Title}\t{page.Title}.txt");
            }
            _pageFiles[page.ID] = new FileInfo(Path.Combine(Context.PageFolder.FullName, $"{page.Title}.txt"));
            writePage(page);
            using (var writer = new StreamWriter(Context.AuthorsFile.FullName, true))
            {
                writer.WriteLine();
                writer.WriteLine($"{page.ID}\t{page.Author.ID}");
            }
        }

        public Page GetPage(Guid id)
        {
            return _backingStore.GetPage(id);
        }

        public Page GetPage(string title)
        {
            return _backingStore.GetPage(title);
        }

        public bool PageExists(string title)
        {
            return _backingStore.PageExists(title);
        }

        public void UpdatePage(Page page)
        {
            _backingStore.UpdatePage(page);
            writePage(page);
        }

        private void writePage(Page page)
        {
            using (var writer = new StreamWriter(_pageFiles[page.ID].FullName, false))
            {
                writer.Write(page.Serialized());
            }
        }

        private void updateAuthors()
        {
            using (var writer = new StreamWriter(Context.AuthorsFile.FullName, false))
            {
                foreach (var page in Pages)
                {
                    writer.WriteLine($"{page.ID}\t{page.Author.ID}");
                }
            }
        }
    }
}
