using System.IO;
using System.IO.Compression;
using Backups.Entities;
using Backups.Models;

namespace Backups.Tools
{
    public static class ZipArchiver
    {
        public static void ArchiveSingleMode(RestorePoint restorePoint, string path)
        {
            Directory.CreateDirectory(path ?? throw new BackupsException("Null argument"));
            foreach (FileData fileData in restorePoint.LocalFileDatas)
            {
                File.Move(fileData.FullPath, path);
            }

            // TODO:проверить, что оно работает
            string pathZip = @$"{path}.zip";
            ZipFile.CreateFromDirectory(path, pathZip);
            Directory.Delete(path);
        }

        public static void ArchiveSplitMode(RestorePoint restorePoint, string path)
        {
            // TODO:называть каждый сжатый файл "{папка, где он был} {название файла}.zip"
        }
    }
}