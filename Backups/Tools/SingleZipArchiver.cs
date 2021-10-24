using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SingleZipArchiver : IArchiver
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

            if (!path.EndsWith(".zip"))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += @"\archive.zip";
            }

            string tempDirPath = $"{path}temp";
            SafeCreateDirectory(tempDirPath);
            foreach (FileInfo fileInfo in fileInfos)
            {
                fileInfo.CopyTo($@"{tempDirPath}\{fileInfo.Name}");
            }

            SafeCreateZipFile(tempDirPath, path);

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