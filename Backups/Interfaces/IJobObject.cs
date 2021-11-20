using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IJobObject
    {
        IReadOnlyList<FileInfo> FileInfos { get; }

        void AddFile(FileInfo fileInfo);
        void RemoveFile(FileInfo fileInfo);
    }
}