using System;
using Backups.Interfaces;

namespace Backups.Entities
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}