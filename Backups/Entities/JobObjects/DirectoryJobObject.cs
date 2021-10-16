using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.JobObjects
{
    public class DirectoryJobObject : IJobObject
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryJobObject(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => _directoryInfo.GetFiles();

        public void RemoveFile(FileInfo fileInfo)
        {
            File.Delete(fileInfo.FullName);
        }
    }
}