using System.Collections.Generic;
using Reports.Entities.Employees;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Entities.Reports
{
    public class EmployeeReport
    {
        private readonly List<Task> _tasks;

        public EmployeeReport(List<Task> tasks, BaseEmployee creator)
        {
            _tasks = tasks ?? throw new ReportsExceptions("Tasks are null");
            Creator = creator ?? throw new ReportsExceptions("Creator is null");
        }

        public IReadOnlyList<Task> Tasks => _tasks;
        public BaseEmployee Creator { get; private set; }
    }
}