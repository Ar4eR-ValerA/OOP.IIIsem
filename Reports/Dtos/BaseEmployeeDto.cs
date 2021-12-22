using System;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Dtos
{
    public abstract class BaseEmployeeDto
    {
        protected BaseEmployeeDto(Guid id, string name, bool active)
        {
            Id = id;
            Name = name ?? throw new ReportsExceptions("Name is null");
            Active = active;
        }
        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool Active { get; private set; }
        public abstract BaseEmployee GetInstance();
    }
}