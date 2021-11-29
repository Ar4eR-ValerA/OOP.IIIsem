using System;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities
{
    public class ConsoleLogger : ILogger
    {
        [JsonConstructor]
        public ConsoleLogger()
        {
        }

        public ConsoleLogger(object extraInfo)
        {
            ExtraInfo = extraInfo ?? throw new BackupsException("Extra info is null");
        }

        [JsonProperty]
        public object ExtraInfo { get; private set; }

        public void Log(string message)
        {
            Console.WriteLine($"{message}: {ExtraInfo}\n");
        }
    }
}