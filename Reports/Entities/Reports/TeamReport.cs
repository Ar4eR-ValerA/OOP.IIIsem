using System.Collections.Generic;

namespace Reports.Entities.Reports
{
    public class TeamReport
    {
        private readonly List<EmployeeReport> _reports;

        public TeamReport()
        {
            _reports = new List<EmployeeReport>();
        }

        public IReadOnlyList<EmployeeReport> Reports => _reports;
    }
}