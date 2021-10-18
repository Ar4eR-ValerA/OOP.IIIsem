namespace Backups.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Receiver.ReceiveFile(8888);
        }
    }
}