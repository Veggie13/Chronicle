using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle.Tests
{
    [TestClass]
    public class ContentTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var pageStore = new PageStore();
            pageStore.AddPage(new Page() { Title = "links" });
            pageStore.AddPage(new Page() { Title = "more" });
            pageStore.AddPage(new Page() { Title = "overformattedness" });

            var content = Content.Parse("Basic text", pageStore);
            Assert.AreEqual(0, content.Children.Count);
            Assert.AreEqual("Basic text", content.Text);

            content = Content.Parse("There *you* are you **ugly** piece of ***text*** full of [[links]] with [[more|alt-text]] and **[[overformattedness]]**", pageStore);
            Assert.AreEqual(12, content.Children.Count);
        }
    }
}
