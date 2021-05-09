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

            var lines = new List<string>();
            using (var reader = new StreamReader(Context.UserListFile.FullName))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var cells = line.Split('\t');

                var user = new User()
                {
                    Name = cells[0],
                    Password = HashedPassword.FromHash(cells[1])
                };
                _users[user.Name] = user;
            }
        }

        public FileApplicationContext Context { get; private set; }

        public User GetUser(string userName)
        {
            return _users[userName];
        }
    }
}
