using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IJobObject
    {
        public IReadOnlyList<FileInfo> FileInfos { get; }

        public void RemoveFile(FileInfo fileInfo);
    }
}