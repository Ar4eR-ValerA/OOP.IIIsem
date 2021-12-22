using System.Collections.Generic;
using System.Linq;
using Reports.Dtos;
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

        internal TeamLead(TeamLeadDto teamLeadDto)
            : base(teamLeadDto.Id, teamLeadDto.Name, teamLeadDto.Active)
        {
            _subordinates = new List<BaseEmployee>();
            foreach (BaseEmployeeDto baseEmployeeDto in teamLeadDto.Subordinates)
            {
                if (baseEmployeeDto is ManagerDto managerDto)
                {
                    _subordinates.Add(new Manager(managerDto));
                }

                if (baseEmployeeDto is EmployeeDto employeeDto)
                {
                    _subordinates.Add(new Employee(employeeDto));
                }
            }
        }

        public virtual IReadOnlyList<BaseEmployee> Subordinates => _subordinates;

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

            if (_subordinates.Contains(subordinate))
            {
                throw new ReportsExceptions("This employee is already subordinate");
            }

            _subordinates.Add(subordinate);
        }

        public override BaseEmployeeDto GetDto()
        {
            return new TeamLeadDto(Id, Name, _subordinates.Select(e => e.GetDto()).ToList(), Active);
        }
    }
}