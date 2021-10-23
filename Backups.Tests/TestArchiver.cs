using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Tests
{
    public class TestArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path)
        {
        }
    }
}