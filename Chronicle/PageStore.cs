using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class PageStore : IPageStore
    {
        private Dictionary<Guid, Page> _pages = new Dictionary<Guid, Page>();
        private Dictionary<string, Guid> _pagesByTitle = new Dictionary<string, Guid>();
        private Dictionary<Guid, string> _titlesByPage = new Dictionary<Guid, string>();

        public IEnumerable<Page> Pages { get { return _pages.Values; } }

        public bool PageExists(string title)
        {
            return _pagesByTitle.ContainsKey(title);
        }

        public void AddPage(Page page)
        {
            if (_pagesByTitle.ContainsKey(page.Title))
            {
                throw new Exception("Duplicate page title.");
            }
            if (page.ID == Guid.Empty)
            {
                page.ID = Guid.NewGuid();
            }
            _pages[page.ID] = page;
            _pagesByTitle[page.Title] = page.ID;
            _titlesByPage[page.ID] = page.Title;
        }

        public Page GetPage(string title)
        {
            try
            {
                return _pages[_pagesByTitle[title]];
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid page title.", ex);
            }
        }

        public void UpdatePage(Page page)
        {
            if (_titlesByPage[page.ID] != page.Title)
            {
                if (_pagesByTitle.ContainsKey(page.Title))
                {
                    throw new Exception("Duplicate Title in page.");
                }
                _pagesByTitle.Remove(_titlesByPage[page.ID]);
                _pagesByTitle[page.Title] = page.ID;
                _titlesByPage[page.ID] = page.Title;
            }
        }
    }
}
