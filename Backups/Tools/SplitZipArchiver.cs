using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SplitZipArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<string> filePaths, string targetPath)
        {
            if (filePaths is null)
            {
                throw new BackupsException("File paths is null");
            }

            if (targetPath is null)
            {
                throw new BackupsException("Target path is null");
            }

            string tempDirPath = $@"{targetPath}\TempDir";
            SafeCreateDirectory(tempDirPath);

            foreach (string filePath in filePaths)
            {
                string tempFullPath = $@"{tempDirPath}\{Path.GetFileName(filePath)}";
                File.Copy(filePath, tempFullPath);

                string pathZip = @$"{targetPath}\{Path.GetFileName(filePath)}.zip";
                SafeCreateZipFile(tempDirPath, pathZip);

                File.Delete(tempFullPath);
            }

            Directory.Delete(tempDirPath, true);
        }

        private static void SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path ?? throw new BackupsException("Path is null")))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        private static void SafeCreateZipFile(string targetPath, string zipPath)
        {
            if (File.Exists(zipPath ?? throw new BackupsException("ZipPath is null")))
            {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(targetPath, zipPath);
        }
    }
}