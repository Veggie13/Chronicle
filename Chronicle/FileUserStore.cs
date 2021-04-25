using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chronicle
{
    public class FileUserStore : IUserStore
    {
        private Dictionary<string, User> _users = new Dictionary<string, User>();

        public FileUserStore(FileApplicationContext context)
        {
            Context = context;

            var userNames = new List<string>();
            using (var reader = new StreamReader(Context.UserListFile.FullName))
            {
                while (!reader.EndOfStream)
                {
                    userNames.Add(reader.ReadLine());
                }
            }

            foreach (string userName in userNames)
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    continue;
                }

                var user = new User() { Name = userName };
                _users[userName] = user;
            }
        }

        public FileApplicationContext Context { get; private set; }

        public User GetUser(string userName)
        {
            return _users[userName];
        }
    }
}
