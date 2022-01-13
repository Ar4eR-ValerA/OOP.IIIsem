using System;
using System.Collections.Generic;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Dtos
{
    public class ManagerDto : BaseEmployeeDto
    {
        private readonly List<EmployeeDto> _subordinates;
        
        public ManagerDto(Guid id, string name, List<EmployeeDto> subordinates, bool active)
            : base(id, name ?? throw new ReportsExceptions("Name is null"), active)
        {
            _subordinates = subordinates ?? throw new ReportsExceptions("Subordinates are null");
        }
        
        public IReadOnlyList<EmployeeDto> Subordinates => _subordinates;

        public override BaseEmployee GetInstance()
        {
            return new Manager(this);
        }
    }
}