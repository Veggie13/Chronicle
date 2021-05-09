using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronicle.Tests
{
    [TestClass]
    public class PasswordTests
    {
        static IEnumerable<object[]> generatePasswords()
        {
            string alphabet = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>? ";
            var rand = new Random((int)0x0dedbeef);
            for (int i = 0; i < 1000; i++)
            {
                int length = rand.Next(1, 64);
                yield return new[] { new string(Enumerable.Range(0, length).Select(_ => alphabet[rand.Next(0, alphabet.Length)]).ToArray()) };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(generatePasswords), DynamicDataSourceType.Method)]
        public void Test(string rawPassword)
        {
            var pw = HashedPassword.Create(rawPassword);
            Assert.AreEqual(pw, rawPassword);
        }
    }
}
