using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chronicle
{
    public class FilePageStore : IPageStore
    {
        private Dictionary<string, Page> _namedPages = new Dictionary<string, Page>();

        public FilePageStore(FileApplicationContext context, IUserStore userStore)
        {
            Context = context;

            foreach (var pageInfo in Context.PageFolder.GetFiles("*.txt"))
            {
                string content;
                using (var reader = new StreamReader(pageInfo.FullName))
                {
                    content = reader.ReadToEnd();
                }

                var page = Page.Parse(content, userStore);
                page.Title = Path.GetFileNameWithoutExtension(pageInfo.Name);
                _namedPages[page.Title] = page;
            }
        }

        public FileApplicationContext Context { get; private set; }

        public Page GetPage(string title)
        {
            return _namedPages[title];
        }
    }
}
