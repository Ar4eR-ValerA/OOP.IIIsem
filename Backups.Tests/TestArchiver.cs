using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Tests
{
    public class TestArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path)
        {
            if (path is null)
            {
                throw new BackupsException("Path is null");
            }

            if (fileInfos is null)
            {
                throw new BackupsException("FileInfos is null");
            }
        }
    }
}