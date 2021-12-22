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
    [Route("/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly IEmployeesService _employeesService;

        public TasksController(ITasksService tasksService, IEmployeesService employeesService)
        {
            _tasksService = tasksService;
            _employeesService = employeesService;
        }

        [HttpPost]
        [Route("/tasks/create")]
        public void CreateTask(
            [FromQuery] string name, 
            [FromQuery] string description, 
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(creatorDtos);
            
            _tasksService.CreateTask(name, description, creatorDtos.First().Id, DateTime.Now);
        }

        [HttpGet]
        [Route("/tasks/find")]
        public IReadOnlyList<TaskDto> Find(
            [FromQuery] string name,
            [FromQuery] Guid id,
            [FromQuery] Guid assignedId,
            [FromQuery] DateTime creationDate,
            [FromQuery] DateTime lastChangeDate)
        {
            return _tasksService.Find(name, id, assignedId, creationDate, lastChangeDate);
        }

        [HttpPatch]
        [Route("/tasks/assign-employee")]
        public void AssignEmployee(
            [FromQuery] string taskName,
            [FromQuery] Guid taskId,
            [FromQuery] string employeeName,
            [FromQuery] Guid employeeId,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<TaskDto> taskDtos = _tasksService.Find(taskName, taskId);
            IReadOnlyList<BaseEmployeeDto> employeeDtos = _employeesService.Find(employeeName, employeeId);
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);

            IsOne(taskDtos);
            IsOne(employeeDtos);
            IsOne(creatorDtos);

            _tasksService.AssignEmployee(
                taskDtos.First().Id, 
                employeeDtos.First().Id, 
                creatorDtos.First().Id, 
                DateTime.Now);
        }

        [HttpPatch]
        [Route("/tasks/add-comment")]
        public void AddComment(
            [FromQuery] string taskName,
            [FromQuery] Guid taskId,
            [FromQuery] string commentName,
            [FromQuery] string commentMessage,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<TaskDto> taskDtos = _tasksService.Find(taskName, taskId);
            IReadOnlyList<BaseEmployeeDto> employeeDtos = _employeesService.Find(creatorName, creatorId);

            IsOne(taskDtos);
            IsOne(employeeDtos);

            _tasksService.AddComment(
                taskDtos.First().Id,
                commentName, commentMessage,
                employeeDtos.First().Id,
                DateTime.Now);
        }

        [HttpPatch]
        [Route("/tasks/finish-task")]
        public void FinishTask(
            [FromQuery] string taskName, 
            [FromQuery] Guid taskId, 
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<TaskDto> taskDtos = _tasksService.Find(taskName, taskId);
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(taskDtos);
            IsOne(creatorDtos);

            _tasksService.FinishTask(taskDtos.First().Id, creatorDtos.First().Id, DateTime.Now);
        }
        
        [HttpPatch]
        [Route("/tasks/open-task")]
        public void OpenTask(
            [FromQuery] string taskName, 
            [FromQuery] Guid taskId, 
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<TaskDto> taskDtos = _tasksService.Find(taskName, taskId);
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(taskDtos);
            IsOne(creatorDtos);

            _tasksService.OpenTask(taskDtos.First().Id, creatorDtos.First().Id, DateTime.Now);
        }
        
        [HttpPatch]
        [Route("/tasks/change-description")]
        public void ChangeDescription(
            [FromQuery] string taskName, 
            [FromQuery] Guid taskId, 
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            IReadOnlyList<TaskDto> taskDtos = _tasksService.Find(taskName, taskId);
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(taskDtos);
            IsOne(creatorDtos);

            _tasksService.OpenTask(taskDtos.First().Id, creatorDtos.First().Id, DateTime.Now);
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