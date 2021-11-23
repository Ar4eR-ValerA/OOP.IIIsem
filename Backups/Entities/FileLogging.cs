using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class FileLogging : ILogging
    {
        public FileLogging(string filePath)
        {
            FilePath = filePath ?? throw new BackupsException("File path is null");
        }

        public string FilePath { get; }

        public void Log(string message)
        {
            File.AppendAllText(FilePath, message);
        }
    }
}