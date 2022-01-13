using System;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Dtos
{
    public class EmployeeDto : BaseEmployeeDto
    { 
        public EmployeeDto(Guid id, string name, bool active)
            : base(id, name ?? throw new ReportsExceptions("Name is null"), active)
        {
        }

        public override BaseEmployee GetInstance()
        {
            return new Employee(this);
        }
    }
}