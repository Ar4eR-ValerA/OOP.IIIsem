using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path);
    }
}