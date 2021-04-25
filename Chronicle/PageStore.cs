using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public class PageStore
    {
        private Dictionary<string, Page> _namedPages = new Dictionary<string, Page>();

        public void AddPage(Page page)
        {
            _namedPages[page.Title] = page;
        }

        public Page GetPage(string title)
        {
            return _namedPages[title];
        }
    }
}
