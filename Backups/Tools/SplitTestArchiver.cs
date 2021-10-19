using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SplitTestArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path)
        {
        }
    }
}