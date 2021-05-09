using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chronicle
{
    public class FileUserStore : IUserStore
    {
        private IUserStore _backingStore;

        public FileUserStore(FileApplicationContext context)
            : this(context, new UserStore())
        { }

        public FileUserStore(FileApplicationContext context, IUserStore backingStore)
        {
            _backingStore = backingStore;
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

                _backingStore.AddUser(new User()
                {
                    Name = cells[0],
                    Password = HashedPassword.FromHash(cells[1])
                });
            }
        }

        public FileApplicationContext Context { get; private set; }
        public IEnumerable<User> Users { get { return _backingStore.Users; } }

        public bool HasUser(string userName)
        {
            return _backingStore.HasUser(userName);
        }

        public User GetUser(string userName)
        {
            return _backingStore.GetUser(userName);
        }

        public void AddUser(User user)
        {
            _backingStore.AddUser(user);
            
            using (var writer = new StreamWriter(Context.UserListFile.FullName, false))
            {
                writer.WriteLine($"{user.Name}\t{user.Password}");
            }
        }
    }
}
