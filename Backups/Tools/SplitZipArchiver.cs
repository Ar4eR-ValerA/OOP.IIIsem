using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SplitZipArchiver : IArchiver
    {
        public void Archive(IReadOnlyList<FileInfo> fileInfos, string path)
        {
            if (path is null || fileInfos is null)
            {
                throw new BackupsException("Null argument");
            }

            string tempDirPath = $@"{path}\TempDir";
            SafeCreateDirectory(tempDirPath);

            foreach (FileInfo fileInfo in fileInfos)
            {
                string tempFullPath = $@"{tempDirPath}\{fileInfo.Name}";
                fileInfo.CopyTo(tempFullPath);

                string pathZip = @$"{path}\{fileInfo.Name}.zip";
                SafeCreateZipFile(tempDirPath, pathZip);

                File.Delete(tempFullPath);
            }

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