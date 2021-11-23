using System;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

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

        public object ExtraInfo { get; private set; }

        public void Log(string message)
        {
            Console.WriteLine($"{message}: {ExtraInfo}\n");
        }
    }
}