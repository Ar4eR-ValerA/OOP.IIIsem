using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.Dtos;
using Reports.Services;

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
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.CreateTask(name, description, creatorDto.Id, DateTime.Now);
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
            TaskDto taskDto = _tasksService.FindOne(taskName, taskId);
            BaseEmployeeDto employeeDto = _employeesService.FindOne(employeeName, employeeId);
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.AssignEmployee(taskDto.Id, employeeDto.Id, creatorDto.Id, DateTime.Now);
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
            TaskDto taskDto = _tasksService.FindOne(taskName, taskId);
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.AddComment(taskDto.Id, commentName, commentMessage, creatorDto.Id, DateTime.Now);
        }

        [HttpPatch]
        [Route("/tasks/finish-task")]
        public void FinishTask(
            [FromQuery] string taskName,
            [FromQuery] Guid taskId,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            TaskDto taskDto = _tasksService.FindOne(taskName, taskId);
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.FinishTask(taskDto.Id, creatorDto.Id, DateTime.Now);
        }

        [HttpPatch]
        [Route("/tasks/open-task")]
        public void OpenTask(
            [FromQuery] string taskName,
            [FromQuery] Guid taskId,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            TaskDto taskDto = _tasksService.FindOne(taskName, taskId);
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.OpenTask(taskDto.Id, creatorDto.Id, DateTime.Now);
        }

        [HttpPatch]
        [Route("/tasks/change-description")]
        public void ChangeDescription(
            [FromQuery] string taskName,
            [FromQuery] Guid taskId,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId)
        {
            TaskDto taskDto = _tasksService.FindOne(taskName, taskId);
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _tasksService.OpenTask(taskDto.Id, creatorDto.Id, DateTime.Now);
        }
    }
}