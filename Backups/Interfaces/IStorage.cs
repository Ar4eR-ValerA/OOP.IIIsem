using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IStorage
    {
        IReadOnlyList<FileInfo> FileInfos { get; }
        string Path { get; }
    }
}