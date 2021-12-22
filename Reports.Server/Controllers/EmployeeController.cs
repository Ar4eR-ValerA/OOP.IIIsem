using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Dtos;
using Reports.Services;
using Reports.Tools;

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
            IReadOnlyList<BaseEmployeeDto> baseTargetEmployeeDtos = _employeesService.Find(targetName, targetId);
            IReadOnlyList<BaseEmployeeDto> supervisorDtos = _employeesService.Find(supervisorName, supervisorId);
            
            IsOne(baseTargetEmployeeDtos);
            IsOne(supervisorDtos);

            _employeesService.ConnectEmployee(baseTargetEmployeeDtos.First().Id, supervisorDtos.First().Id);
        }
        
        [HttpPatch]
        [Route("/employees/delete-employee")]
        public void ConnectEmployee(
            [FromQuery] string name,
            [FromQuery] Guid id)
        {
            IReadOnlyList<BaseEmployeeDto> employeeDtos = _employeesService.Find(name, id);
            IsOne(employeeDtos);

            _employeesService.DeleteEmployee(employeeDtos.First().Id);
        }
        
        private void IsOne(IReadOnlyList<object> objects)
        {
            if (objects is null)
            {
                throw new ReportsExceptions("There is no objects");
            }

            if (objects.Count == 0)
            {
                throw new ReportsExceptions(
                    $"There is not such {objects.GetType().GetGenericArguments().First().Name}");
            }

            if (objects.Count > 1)
            {
                throw new ReportsExceptions($"There are more than 1 suitable {objects.First().GetType().Name}");
            }
        }
    }
}