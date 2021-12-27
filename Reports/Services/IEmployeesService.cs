using System;
using System.Collections.Generic;
using Reports.Dtos;

namespace Reports.Services
{
    public interface IEmployeesService
    {
        void CreateEmployee(string name);
        void CreateManager(string name);
        void CreateTeamLead(string name);
        void DeleteEmployee(Guid id);
        IReadOnlyList<BaseEmployeeDto> FindByName(string name);
        IReadOnlyList<BaseEmployeeDto> FindById(Guid id);
        IReadOnlyList<BaseEmployeeDto> Find(string name, Guid id);
        BaseEmployeeDto FindOne(string name, Guid id);
        IReadOnlyList<BaseEmployeeDto> GetAllEmployees();
        void ConnectEmployee(Guid targetEmployeeId, Guid supervisorId);
    }
}