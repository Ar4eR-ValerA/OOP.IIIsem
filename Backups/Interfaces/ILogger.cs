namespace Backups.Interfaces
{
    public interface ILogger
    {
        object ExtraInfo { get; }
        void Log(string message);
    }
}