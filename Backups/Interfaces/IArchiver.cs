using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
        void Archive(IReadOnlyList<string> filePaths, string targetPath);
        void Unpack(string archivePath, string targetPath);
    }
}