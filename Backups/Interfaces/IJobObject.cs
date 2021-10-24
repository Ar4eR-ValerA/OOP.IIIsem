using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IJobObject
    {
        IReadOnlyList<FileInfo> FileInfos { get; }

        void RemoveFile(FileInfo fileInfo);
    }
}