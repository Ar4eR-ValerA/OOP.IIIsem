using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Tests
{
    public class TestArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<string> filePaths, string targetPath)
        {
            if (targetPath is null)
            {
                throw new BackupsException("Path is null");
            }

            if (filePaths is null)
            {
                throw new BackupsException("File paths is null");
            }
        }

        public void Unpack(string archivePath, string targetPath)
        {
            if (archivePath is null)
            {
                throw new BackupsException("Archive path is null");
            }

            if (targetPath is null)
            {
                throw new BackupsException("Target path is null");
            }
        }
    }
}