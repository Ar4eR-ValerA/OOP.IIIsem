using System;
using Reports.Dtos;
using Reports.Tools;

namespace Reports.Entities.Employees
{
    public abstract class BaseEmployee
    {
        protected BaseEmployee(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ReportsExceptions("Name is null");
            Active = true;
        }

        protected BaseEmployee(Guid id, string name, bool active)
        {
            Id = id;
            Name = name ?? throw new ReportsExceptions("Name is null");
            Active = active;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public bool Active { get; set; }

        public abstract BaseEmployeeDto GetDto();
    }
}