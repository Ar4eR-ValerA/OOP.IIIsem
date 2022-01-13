using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Dtos;
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

        internal Manager(ManagerDto managerDto)
            : base(managerDto.Id, managerDto.Name, managerDto.Active)
        {
            _subordinates = managerDto.Subordinates.Select(e => new Employee(e)).ToList();
        }

        public virtual IReadOnlyList<Employee> Subordinates => _subordinates;

        public void AddSubordinate(BaseEmployee subordinate)
        {
            if (subordinate is null)
            {
                throw new ReportsExceptions("Subordinate is null");
            }

            if (subordinate is not Employee employee)
            {
                throw new ReportsExceptions("Subordinate of manager must be employee");
            }

            if (_subordinates.Contains(subordinate))
            {
                throw new ReportsExceptions("This employee is already subordinate");
            }

            _subordinates.Add(employee);
        }

        public override BaseEmployeeDto GetDto()
        {
            return new ManagerDto(Id, Name, _subordinates.Select(e => e.GetDto() as EmployeeDto).ToList(), Active);
        }
    }
}