using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SingleZipArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<string> filePaths, string targetPath)
        {
            if (targetPath is null)
            {
                throw new BackupsException("Path is null");
            }

            if (filePaths is null)
            {
                throw new BackupsException("FileInfos is null");
            }

            if (!targetPath.EndsWith(".zip"))
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                targetPath += @"\archive.zip";
            }

            string tempDirPath = $"{targetPath}temp";
            SafeCreateDirectory(tempDirPath);
            foreach (string filePath in filePaths)
            {
                File.Copy(filePath, $@"{tempDirPath}\{Path.GetFileName(filePath)}");
            }

            SafeCreateZipFile(tempDirPath, targetPath);

            Directory.Delete(tempDirPath, true);
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

            SafeCreateZipFile(targetPath, archivePath);
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