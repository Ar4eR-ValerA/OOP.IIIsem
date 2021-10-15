using System.IO;
using System.IO.Compression;
using Backups.Entities;
using Backups.Entities.Storages;

namespace Backups.Tools
{
    public static class ZipArchiver
    {
        /// <summary>
        /// Archiving files from restore point to single zip file that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path"> Path must points to .zip file. </param>
        public static void ArchiveSingleMode(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            if (!path.EndsWith(".zip"))
            {
                throw new BackupsException("Path must ends with .zip file");
            }

            string tempDirPath = $"{path}temp";
            SafeCreateDirectory(tempDirPath);
            foreach (FileInfo fileInfo in restorePoint.LocalFileInfos)
            {
                fileInfo.CopyTo($@"{tempDirPath}\{fileInfo.Name}");
            }

            SafeCreateZipFile(tempDirPath, path);
            restorePoint.AddStorage(new FileStorage(new FileInfo(path)));

            Directory.Delete(tempDirPath, true);
        }

        /// <summary>
        /// Archiving files from restore point to several zip files in directory that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path"> Path must points to directory where zip files will be located. </param>
        public static void ArchiveSplitMode(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            if (!Directory.Exists(path))
            {
                throw new BackupsException("Path must points to directory");
            }

            string tempDirPath = $@"{path}\TempDir";
            SafeCreateDirectory(tempDirPath);

            foreach (FileInfo fileInfo in restorePoint.LocalFileInfos)
            {
                string tempFullPath = $@"{tempDirPath}\{fileInfo.Name}";
                fileInfo.CopyTo(tempFullPath);

                string pathZip = @$"{path}\{fileInfo.Name}.zip";
                SafeCreateZipFile(tempDirPath, pathZip);
                restorePoint.AddStorage(new FileStorage(new FileInfo(pathZip)));

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