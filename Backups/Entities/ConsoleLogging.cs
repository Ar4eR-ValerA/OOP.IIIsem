using System;
using Backups.Interfaces;

namespace Backups.Entities
{
    public class ConsoleLogging : ILogging
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}