using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SingleTestArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path)
        {
        }
    }
}