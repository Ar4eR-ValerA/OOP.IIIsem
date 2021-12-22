using System.Collections.Generic;
using Reports.Tools;

namespace Reports.Entities.Employees
{
    public class Manager : BaseEmployee
    {
        private readonly List<Employee> _subordinates;

        public Manager(string name)
            : base(name)
        {
            _subordinates = new List<Employee>();
        }

        public IReadOnlyList<Employee> Subordinates => _subordinates;

        public void AddSubordinate(Employee subordinate)
        {
            if (subordinate is null)
            {
                throw new ReportsExceptions("Subordinate is null");
            }

            _subordinates.Add(subordinate);
        }
    }
}