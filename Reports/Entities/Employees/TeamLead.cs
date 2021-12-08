using System.Collections.Generic;
using Reports.Tools;

namespace Reports.Entities.Employees
{
    public class TeamLead : BaseEmployee
    {
        private readonly List<BaseEmployee> _subordinates;

        public TeamLead(string name)
            : base(name)
        {
            _subordinates = new List<BaseEmployee>();
        }

        public IReadOnlyList<BaseEmployee> Subordinates => _subordinates;

        public void AddSubordinate(BaseEmployee subordinate)
        {
            if (subordinate is null)
            {
                throw new ReportsExceptions("Subordinate is null");
            }

            if (subordinate is TeamLead)
            {
                throw new ReportsExceptions("You can't add team lead as a subordinate");
            }

            _subordinates.Add(subordinate);
        }
    }
}