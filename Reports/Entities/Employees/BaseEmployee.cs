using System;
using Reports.Tools;

namespace Reports.Entities.Employees
{
    public abstract class BaseEmployee
    {
        protected BaseEmployee(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ReportsExceptions("Name is null");
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}