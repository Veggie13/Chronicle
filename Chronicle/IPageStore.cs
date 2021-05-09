using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public interface IPageStore
    {
        IEnumerable<Page> Pages { get; }

        bool PageExists(string title);
        Page GetPage(string title);
        void AddPage(Page page);
        void UpdatePage(Page page);
    }
}
