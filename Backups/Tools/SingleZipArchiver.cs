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
            if (path is null || fileInfos is null)
            {
                throw new BackupsException("Null argument");
            }

            if (!path.EndsWith(".zip"))
            {
                if (!new DirectoryInfo(path).Exists)
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
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        private static void SafeCreateZipFile(string targetPath, string zipPath)
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(targetPath, zipPath);
        }
    }
}