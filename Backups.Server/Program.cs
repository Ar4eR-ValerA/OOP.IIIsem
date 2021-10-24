using System.Net;

namespace Backups.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Receiver.ReceiveFile(IPAddress.Parse("127.0.0.1"), 8888);
        }
    }
}