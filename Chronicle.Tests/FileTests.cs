using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle.Tests
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        [DeploymentItem("testRepo")]
        public void LoadTest()
        {
            var context = new FileApplicationContext(@"testRepo");
            var userStore = new FileUserStore(context);
            var pageStore = new FilePageStore(context, userStore);

            var page1 = pageStore.GetPage("Page1");
            string page1String = page1.ToString();

            var page2 = pageStore.GetPage("Page2");
            string page2String = page2.ToString();

            var page3 = pageStore.GetPage("Page3");
            string page3String = page3.ToString();
        }
    }
}
