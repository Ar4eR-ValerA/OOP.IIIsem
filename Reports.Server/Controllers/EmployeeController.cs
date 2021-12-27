using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Dtos;
using Reports.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeeController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpPost]
        [Route("/employees/create/employee")]
        public void CreateEmployee([FromQuery] string name)
        {
            _employeesService.CreateEmployee(name);
        }

        [HttpPost]
        [Route("/employees/create/manager")]
        public void CreateManager([FromQuery] string name)
        {
            _employeesService.CreateManager(name);
        }

        [HttpPost]
        [Route("/employees/create/teamlead")]
        public void CreateTeamLead([FromQuery] string name)
        {
            _employeesService.CreateTeamLead(name);
        }

        [HttpGet]
        [Route("/employees/find")]
        public IReadOnlyList<BaseEmployeeDto> Find([FromQuery] string name, [FromQuery] Guid id)
        {
            return _employeesService.Find(name, id);
        }

        [HttpPatch]
        [Route("/employees/employee-connection")]
        public void ConnectEmployee(
            [FromQuery] string targetName,
            [FromQuery] Guid targetId,
            [FromQuery] string supervisorName,
            [FromQuery] Guid supervisorId)
        {
            BaseEmployeeDto baseTargetEmployeeDto = _employeesService.FindOne(targetName, targetId);
            BaseEmployeeDto supervisorDto = _employeesService.FindOne(supervisorName, supervisorId);

            _employeesService.ConnectEmployee(baseTargetEmployeeDto.Id, supervisorDto.Id);
        }
        
        [HttpPatch]
        [Route("/employees/delete-employee")]
        public void ConnectEmployee(
            [FromQuery] string name,
            [FromQuery] Guid id)
        {
            BaseEmployeeDto employeeDto = _employeesService.FindOne(name, id);

            _employeesService.DeleteEmployee(employeeDto.Id);
        }
    }
}