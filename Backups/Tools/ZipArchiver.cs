using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class ZipArchiver : IArchiver
    {
        /// <summary>
        /// Archiving files to single zip file that indicated in path.
        /// </summary>
        /// <param name="fileInfos"> Files which archiving. </param>
        /// <param name="path"> Path must points to .zip file, which will be created by method. </param>
        public void ArchiveSingleMode(IReadOnlyList<FileInfo> fileInfos, string path)
        {
            if (path is null || fileInfos is null)
            {
                throw new BackupsException("Null argument");
            }

            if (!path.EndsWith(".zip"))
            {
                throw new BackupsException("Path must ends with .zip file");
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

        /// <summary>
        /// Archiving each file to his own zip file in directory that indicated in path.
        /// </summary>
        /// <param name="fileInfos"> Files which archiving. </param>
        /// <param name="path">
        /// Path must points to directory where zip files will be located. Method will create directory.
        /// </param>
        public void ArchiveSplitMode(IReadOnlyList<FileInfo> fileInfos, string path)
        {
            if (path is null || fileInfos is null)
            {
                throw new BackupsException("Null argument");
            }

            if (!Directory.Exists(path))
            {
                throw new BackupsException("Path must points to directory");
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