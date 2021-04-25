using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chronicle;

namespace Chronicle.Tests
{
    [TestClass]
    public class PermittableTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var userStore = new UserStore();
            userStore.AddUser(new User() { Name = "Alpha" });
            userStore.AddUser(new User() { Name = "Beta" });
            userStore.AddUser(new User() { Name = "Gamma" });
            userStore.AddUser(new User() { Name = "Delta" });

            var permittable = Permittable.Parse("u(a(Alpha),a(Beta))", userStore);
            Assert.AreEqual(permittable.ToString(), "u(a(Alpha),a(Beta))");
            Assert.IsTrue(permittable.IsPermitted(userStore.GetUser("Alpha")));
            Assert.IsTrue(permittable.IsPermitted(userStore.GetUser("Beta")));
            Assert.IsFalse(permittable.IsPermitted(userStore.GetUser("Gamma")));
            Assert.IsFalse(permittable.IsPermitted(userStore.GetUser("Delta")));

            permittable = Permittable.Parse(" e  (   a (  Gamma) )  ", userStore);
            Assert.IsTrue(permittable.IsPermitted(userStore.GetUser("Alpha")));
            Assert.IsTrue(permittable.IsPermitted(userStore.GetUser("Beta")));
            Assert.IsFalse(permittable.IsPermitted(userStore.GetUser("Gamma")));
            Assert.IsTrue(permittable.IsPermitted(userStore.GetUser("Delta")));
        }
    }
}
