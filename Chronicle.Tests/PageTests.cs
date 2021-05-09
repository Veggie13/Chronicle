using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle.Tests
{
    [TestClass]
    public class PageTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var userStore = new UserStore();
            userStore.AddUser(new User() { Name = "Alpha" });
            userStore.AddUser(new User() { Name = "Beta" });
            userStore.AddUser(new User() { Name = "Gamma" });
            userStore.AddUser(new User() { Name = "Delta" });

            var pageStore = new PageStore();
            pageStore.AddPage(new Page() { Title = "Page2" });

            var page = new Page();
            page.Parse(@"
title(Test Page)
editors(a(Beta))

[[[
This is the first block
]]]


This is the naked second block **with** *formatting*

[[[ viewers(a(Gamma))
This block [[Page2|has a link]] and special permissions
]]]

", userStore, pageStore);

        }
    }
}
