﻿using System;
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
            PageFolder = RepositoryFolder.GetDirectories("pages")[0];
            UserListFile = RepositoryFolder.GetFiles("users.txt")[0];
        }

        public DirectoryInfo RepositoryFolder { get; private set; }
        public DirectoryInfo PageFolder { get; private set; }
        public FileInfo UserListFile { get; private set; }
    }
}
