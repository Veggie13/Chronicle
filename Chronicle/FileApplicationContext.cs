using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chronicle
{
    public class FileApplicationContext
    {
        public FileApplicationContext(string repoPath)
        {
            RepositoryFolder = new DirectoryInfo(repoPath);
            PageFolder = new DirectoryInfo(Path.Combine(RepositoryFolder.FullName, "pages"));
            UserListFile = new FileInfo(Path.Combine(RepositoryFolder.FullName, "users.txt"));
            AuthorsFile = new FileInfo(Path.Combine(RepositoryFolder.FullName, "authors.txt"));
            IndexFile = new FileInfo(Path.Combine(RepositoryFolder.FullName, "index.txt"));
        }

        public DirectoryInfo RepositoryFolder { get; private set; }
        public DirectoryInfo PageFolder { get; private set; }
        public FileInfo UserListFile { get; private set; }
        public FileInfo AuthorsFile { get; private set; }
        public FileInfo IndexFile { get; private set; }
    }
}
