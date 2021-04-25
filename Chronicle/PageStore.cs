using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class PageStore : IPageStore
    {
        private Dictionary<string, Page> _namedPages = new Dictionary<string, Page>();
        
        public IEnumerable<Page> Pages { get { return _namedPages.Values; } }

        public bool PageExists(string title)
        {
            return _namedPages.ContainsKey(title);
        }

        public void AddPage(Page page)
        {
            if (_namedPages.ContainsKey(page.Title))
            {
                throw new Exception("Duplicate page title.");
            }

            _namedPages[page.Title] = page;
        }

        public Page GetPage(string title)
        {
            try
            {
                return _namedPages[title];
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid page title.", ex);
            }
        }
    }
}
