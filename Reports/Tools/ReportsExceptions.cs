using System;

namespace Reports.Tools
{
    public class ReportsExceptions : Exception
    {
        public ReportsExceptions()
        {
        }

        public ReportsExceptions(string message)
            : base(message)
        {
        }

        public ReportsExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}