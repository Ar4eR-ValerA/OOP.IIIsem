﻿using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class FileStorage : IStorage
    {
        private readonly FileInfo _fileInfo;

        public FileStorage(FileInfo file)
        {
            _fileInfo = file ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { _fileInfo };
    }
}