using System;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class ConfigurationService
    {
        public ConfigurationService(string configFile)
        {
            ConfigFile = configFile ?? throw new BackupsExtraException("Config file is null");
        }

        public string ConfigFile { get; }

        public void Configure(BackupJobsService backupJobsService)
        {
            throw new NotImplementedException();
        }

        public BackupJobsService Setup()
        {
            throw new NotImplementedException();
        }
    }
}