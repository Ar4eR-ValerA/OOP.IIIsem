using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Reports.Database;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly ReportsDatabaseContext _context;

        public EmployeesService(ReportsDatabaseContext context)
        {
            _context = context;
        }

        public void CreateEmployee(string name)
        {
            if (name is null)
            {
                throw new ReportsExceptions("Name is null");
            }

            var employee = new Employee(name);
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void CreateManager(string name)
        {
            if (name is null)
            {
                throw new ReportsExceptions("Name is null");
            }

            var manager = new Manager(name);
            _context.Managers.Add(manager);
            _context.SaveChanges();
        }

        public void CreateTeamLead(string name)
        {
            if (name is null)
            {
                throw new ReportsExceptions("Name is null");
            }

            var teamLead = new TeamLead(name);
            _context.TeamLeads.Add(teamLead);
            _context.SaveChanges();
        }

        public void DeleteEmployee(Guid id)
        {
            BaseEmployee employee = _context.BaseEmployees.Find(id);

            if (employee is null)
            {
                throw new ReportsExceptions("There is no such employee");
            }

            employee.Active = false;
            _context.BaseEmployees.Update(employee);
            _context.SaveChanges();
        }

        public IReadOnlyList<BaseEmployeeDto> FindByName(string name)
        {
            return _context.BaseEmployees
                .Where(e => e.Name == name && e.Active)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<BaseEmployeeDto> FindById(Guid id)
        {
            return _context.BaseEmployees
                .Where(e => e.Id == id && e.Active)
                .Include(e => e.Active)
                .Select(e => e.GetDto()).ToList();
        }

        public IReadOnlyList<BaseEmployeeDto> Find(string name, Guid id)
        {
            if (name is null && id == Guid.Empty)
            {
                return GetAllEmployees();
            }

            var nameEmployeeDtos = new List<BaseEmployeeDto>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                IReadOnlyList<BaseEmployeeDto> result = FindByName(name);
                nameEmployeeDtos.AddRange(result);
            }

            var idTaskDtos = new List<BaseEmployeeDto>();
            if (id != Guid.Empty)
            {
                IReadOnlyList<BaseEmployeeDto> result = FindById(id);
                idTaskDtos.AddRange(result);
            }

            List<List<BaseEmployeeDto>> notNullLists = GetNotNullLists(nameEmployeeDtos, idTaskDtos);

            if (notNullLists.Count == 0)
            {
                return new List<BaseEmployeeDto>();
            }

            List<BaseEmployeeDto> commonDtos = notNullLists.First();

            return notNullLists
                .Aggregate(commonDtos, (current, baseEmpoloyeeDtos)
                    => current.Intersect(baseEmpoloyeeDtos).ToList());
        }

        public BaseEmployeeDto FindOne(string name, Guid id)
        {
            IReadOnlyList<BaseEmployeeDto> employees = Find(name, id);
            if (employees.Count == 0)
            {
                throw new ReportsExceptions("There are no suitable employees");
            }

            if (employees.Count > 1)
            {
                throw new ReportsExceptions("There are more then 1 suitable employees");
            }

            return employees.Single();
        }

        public IReadOnlyList<BaseEmployeeDto> GetAllEmployees()
        {
            return _context.BaseEmployees
                .Where(e => e.Active)
                .Select(e => e.GetDto()).ToList();
        }

        public void ConnectEmployee(Guid targetEmployeeId, Guid supervisorId)
        {
            BaseEmployee targetEmployee = _context.BaseEmployees.Find(targetEmployeeId);
            BaseEmployee supervisor = _context.BaseEmployees.Find(supervisorId);

            if (targetEmployee is null || !targetEmployee.Active)
            {
                throw new ReportsExceptions("There is no such target employee");
            }

            if (supervisor is null || !supervisor.Active)
            {
                throw new ReportsExceptions("There is no such target supervisor");
            }

            if (supervisor is Employee)
            {
                throw new ReportsExceptions("Standard employee can't be a supervisor");
            }

            if (supervisor is Manager manager)
            {
                ConnectEmployee(targetEmployee, manager);
            }

            if (supervisor is TeamLead teamLead)
            {
                ConnectEmployee(targetEmployee, teamLead);
            }
        }

        private void ConnectEmployee(BaseEmployee targetEmployee, Manager supervisor)
        {
            if (supervisor is null)
            {
                throw new ReportsExceptions("Supervisor is null");
            }

            supervisor.AddSubordinate(targetEmployee);

            _context.Managers.Update(supervisor);
            _context.SaveChanges();
        }

        private void ConnectEmployee(BaseEmployee targetEmployee, TeamLead supervisor)
        {
            if (supervisor is null)
            {
                throw new ReportsExceptions("Supervisor is null");
            }

            supervisor.AddSubordinate(targetEmployee);

            _context.TeamLeads.Update(supervisor);
            _context.SaveChanges();
        }

        private List<List<BaseEmployeeDto>> GetNotNullLists(params List<BaseEmployeeDto>[] list)
        {
            return list.Where(e => e.Count > 0).ToList();
        }
    }
}