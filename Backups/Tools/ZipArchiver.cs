using System.IO;
using System.IO.Compression;
using Backups.Entities;

namespace Backups.Tools
{
    public static class ZipArchiver
    {
        public static void ArchiveSingleMode(RestorePoint restorePoint, string path, string name)
        {
            string fullPath = $@"{path ?? throw new BackupsException("Null argument")}\" +
                              $@"{name ?? throw new BackupsException("Null argument")}";

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }

            Directory.CreateDirectory(fullPath);
            foreach (FileInfo fileInfo in restorePoint.LocalFileInfos)
            {
                fileInfo.CopyTo($@"{fullPath}\{fileInfo.Name}");
            }

            string pathZip = @$"{fullPath}.zip";
            if (File.Exists(pathZip))
            {
                File.Delete(pathZip);
            }

            ZipFile.CreateFromDirectory(fullPath, pathZip);
            Directory.Delete(fullPath, true);
        }

        public static void ArchiveSplitMode(RestorePoint restorePoint, string path)
        {
            if (path is null)
            {
                throw new BackupsException("Null argument");
            }

            string tempDirPath = $@"{path}\TempDir";
            if (Directory.Exists(tempDirPath))
            {
                Directory.Delete(tempDirPath, true);
            }

            Directory.CreateDirectory(tempDirPath);

            foreach (FileInfo fileInfo in restorePoint.LocalFileInfos)
            {
                string tempFullPath = $@"{tempDirPath}\{fileInfo.Name}";
                fileInfo.CopyTo(tempFullPath);

                string pathZip = @$"{path}\{fileInfo.Name}.zip";
                if (File.Exists(pathZip))
                {
                    File.Delete(pathZip);
                }

                ZipFile.CreateFromDirectory(tempDirPath, pathZip);
                File.Delete(tempFullPath);
            }

            Directory.Delete(tempDirPath, true);
        }
    }
}