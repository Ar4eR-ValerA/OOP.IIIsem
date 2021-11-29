using System.IO;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities
{
    public class FileLogger : ILogger
    {
        [JsonConstructor]
        public FileLogger()
        {
            FilePath = "log.txt";
        }

        public FileLogger(string filePath)
        {
            FilePath = filePath ?? throw new BackupsException("File path is null");
        }

        public FileLogger(string filePath, object extraInfo)
        {
            FilePath = filePath ?? throw new BackupsException("File path is null");
            ExtraInfo = extraInfo ?? throw new BackupsException("Extra info is null");
        }

        [JsonProperty]
        public string FilePath { get; private set; }

        [JsonProperty]
        public object ExtraInfo { get; private set; }

        public void Log(string message)
        {
            File.AppendAllText(FilePath, $"{message}: {ExtraInfo}\n");
        }
    }
}