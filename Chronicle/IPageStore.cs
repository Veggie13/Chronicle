using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle
{
    public interface IPageStore
    {
        Page GetPage(string title);
    }
}
