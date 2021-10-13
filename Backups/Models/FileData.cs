using Backups.Tools;

namespace Backups.Models
{
    public class FileData
    {
        public FileData(string name, string destination)
        {
            Name = name ?? throw new BackupsException("Null argument");
            Destination = destination ?? throw new BackupsException("Null argument");
            FullPath = $"{Destination}/{Name}";
        }

        public string Name { get; }
        public string Destination { get; }
        public string FullPath { get; }
    }
}