using Reports.Dtos;

namespace Reports.Entities.Employees
{
    public class Employee : BaseEmployee
    {
        public Employee(string name)
            : base(name)
        {
        }

        internal Employee(EmployeeDto employeeDto)
            : base(employeeDto.Id, employeeDto.Name, employeeDto.Active)
        {
        }

        public override BaseEmployeeDto GetDto()
        {
            return new EmployeeDto(Id, Name, Active);
        }
    }
}