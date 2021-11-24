using System.IO;
using Backups.Entities;
using BackupsExtra.Tools;
using Newtonsoft.Json;

namespace BackupsExtra.Services
{
    public class SerializeBackupJob
    {
        public SerializeBackupJob()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            };
        }

        public JsonSerializerSettings JsonSerializerSettings { get; }

        public void Serialize(BackupJob backupJob, string path)
        {
            if (path is null)
            {
                throw new BackupsExtraException("Path is null");
            }

            if (backupJob is null)
            {
                throw new BackupsExtraException("Backup job is null");
            }

            string jsonString = JsonConvert.SerializeObject(backupJob, Formatting.Indented, JsonSerializerSettings);
            File.WriteAllText(path, jsonString);
        }

        public BackupJob Deserialize(string path)
        {
            if (path is null)
            {
                throw new BackupsExtraException("Path is null");
            }

            string jsonString = File.ReadAllText(path);
            BackupJob backupJob = JsonConvert.DeserializeObject<BackupJob>(jsonString, JsonSerializerSettings);

            if (backupJob is null)
            {
                throw new BackupsExtraException("Json error, there is no backup job");
            }

            return backupJob;
        }
    }
}