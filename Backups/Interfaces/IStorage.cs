using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IStorage
    {
        public IReadOnlyList<FileInfo> FileInfos { get; }
    }
}