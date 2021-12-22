using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Reports.Database;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Services
{
    public class TasksService : ITasksService
    {
        private ReportsDatabaseContext _context;

        public TasksService(ReportsDatabaseContext context)
        {
            _context = context;
        }

        public void CreateTask(string name, string description, Guid creatorId, DateTime creationDate)
        {
            var task = new Task(name, description, creationDate);
            _context.Tasks.Add(task);

            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (creator is null)
            {
                throw new ReportsExceptions("There is no such creator");
            }

            var comment = new Comment(
                "Task was created",
                $"{creator.Name} changed task status to open",
                creator,
                creationDate);

            _context.Comments.Add(comment);
            task.AddComment(comment);

            _context.SaveChanges();
        }

        public IReadOnlyList<TaskDto> FindByName(string name)
        {
            return _context.Tasks
                .Where(e => e.Name == name)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<TaskDto> FindById(Guid id)
        {
            return _context.Tasks
                .Where(e => e.Id == id)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<TaskDto> FindByAssignedId(Guid assignedId)
        {
            return _context.Tasks
                .Where(e => e.Employee.Id == assignedId)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<TaskDto> FindByCreationDate(DateTime creationDate)
        {
            return _context.Tasks
                .Where(e => e.CreationDate == creationDate)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<TaskDto> FindByLastChangeDate(DateTime lastChangeDate)
        {
            return _context.Tasks
                .Where(e => e.LastChangeDate == lastChangeDate)
                .Select(e => e.GetDto())
                .ToList();
        }

        public IReadOnlyList<TaskDto> Find(string name, Guid id)
        {
            var nameTaskDtos = new List<TaskDto>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                IReadOnlyList<TaskDto> result = FindByName(name);
                nameTaskDtos.AddRange(result);
            }

            var idTaskDtos = new List<TaskDto>();
            if (id != Guid.Empty)
            {
                IReadOnlyList<TaskDto> result = FindById(id);
                idTaskDtos.AddRange(result);
            }

            List<List<TaskDto>> notNullLists = GetNotNullLists();

            if (notNullLists.Count == 0)
            {
                return GetAllTasks();
            }

            List<TaskDto> commonDtos = notNullLists.First();

            return notNullLists
                .Aggregate(commonDtos, (current, taskDtos)
                    => current.Intersect(taskDtos).ToList());
        }

        public IReadOnlyList<TaskDto> Find(
            string name,
            Guid id,
            Guid assignedId,
            DateTime creationDate,
            DateTime lastChangeDate)
        {
            if (name is null &&
                id == Guid.Empty &&
                assignedId == Guid.Empty &&
                creationDate == default &&
                lastChangeDate == default)
            {
                return GetAllTasks();
            }

            var nameTaskDtos = new List<TaskDto>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                IReadOnlyList<TaskDto> result = FindByName(name);
                nameTaskDtos.AddRange(result);
            }

            var idTaskDtos = new List<TaskDto>();
            if (id != Guid.Empty)
            {
                IReadOnlyList<TaskDto> result = FindById(id);
                idTaskDtos.AddRange(result);
            }

            var assignedIdTaskDtos = new List<TaskDto>();
            if (assignedId != Guid.Empty)
            {
                IReadOnlyList<TaskDto> result = FindByAssignedId(id);
                assignedIdTaskDtos.AddRange(result);
            }

            var creationDateTaskDtos = new List<TaskDto>();
            if (creationDate != default)
            {
                IReadOnlyList<TaskDto> result = FindByCreationDate(creationDate);
                creationDateTaskDtos.AddRange(result);
            }

            var lastChangeDateTaskDtos = new List<TaskDto>();
            if (lastChangeDate != default)
            {
                IReadOnlyList<TaskDto> result = FindByLastChangeDate(lastChangeDate);
                lastChangeDateTaskDtos.AddRange(result);
            }

            List<List<TaskDto>> notNullLists = GetNotNullLists(
                nameTaskDtos,
                idTaskDtos,
                assignedIdTaskDtos,
                creationDateTaskDtos,
                lastChangeDateTaskDtos);

            if (notNullLists.Count == 0)
            {
                return new List<TaskDto>();
            }

            List<TaskDto> commonDtos = notNullLists.First();

            return notNullLists
                .Aggregate(commonDtos, (current, taskDtos)
                    => current.Intersect(taskDtos).ToList());
        }

        public IReadOnlyList<TaskDto> GetAllTasks()
        {
            return _context.Tasks.Include(t => t.Comments).Select(t => t.GetDto()).ToList();
        }

        public void AddComment(
            Guid taskId,
            string commentName,
            string commentMessage,
            Guid creatorId,
            DateTime creationDate)
        {
            Task task = _context.Tasks.Find(taskId);
            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (task is null)
            {
                throw new ReportsExceptions("Task is null");
            }

            if (creator is null)
            {
                throw new ReportsExceptions("Creator is null");
            }

            var comment = new Comment(commentName, commentMessage, creator, creationDate);
            _context.Comments.Add(comment);
            task.AddComment(comment);

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void ChangeDescription(Guid taskId, string description)
        {
            Task task = _context.Tasks.Find(taskId);

            if (task is null)
            {
                throw new ReportsExceptions("Task is null");
            }

            task.ChangeDescription(description);
            _context.Update(task);
            _context.SaveChanges();
        }

        public void AssignEmployee(Guid taskId, Guid employeeId, Guid creatorId, DateTime assignDate)
        {
            Task task = _context.Tasks.Find(taskId);
            BaseEmployee employee = _context.BaseEmployees.Find(employeeId);
            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (task is null)
            {
                throw new ReportsExceptions("There is no such task");
            }

            if (employee is null)
            {
                throw new ReportsExceptions("There is no such employee");
            }

            if (creator is null)
            {
                throw new ReportsExceptions("There is no such creator");
            }

            var commentStatus = new Comment(
                "Task status change",
                $"{creator.Name} changed task status to active",
                creator,
                assignDate);
            _context.Comments.Add(commentStatus);
            task.AddComment(commentStatus);

            var commentAssign = new Comment(
                "Employee assigning",
                $"{creator.Name} assigned {employee.Name} to this task",
                creator,
                assignDate);
            _context.Comments.Add(commentAssign);
            task.AddComment(commentAssign);

            task.SetActiveStatus();
            task.AssignEmployee(employee);

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void FinishTask(Guid taskId, Guid creatorId, DateTime finishDate)
        {
            Task task = _context.Tasks.Find(taskId);
            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (task is null)
            {
                throw new ReportsExceptions("There is no such task");
            }

            if (creator is null)
            {
                throw new ReportsExceptions("There is no such creator");
            }

            var comment = new Comment(
                "Task status change",
                $"{creator.Name} changed task status to resolved",
                creator,
                finishDate);
            _context.Comments.Add(comment);
            task.AddComment(comment);

            task.SetResolvedStatus();
            task.RemoveEmployee();

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void OpenTask(Guid taskId, Guid creatorId, DateTime finishDate)
        {
            Task task = _context.Tasks.Find(taskId);
            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (task is null)
            {
                throw new ReportsExceptions("There is no such task");
            }

            if (creator is null)
            {
                throw new ReportsExceptions("There is no such creator");
            }

            var comment = new Comment(
                "Task status change",
                $"{creator.Name} changed task status to open",
                creator,
                finishDate);
            _context.Comments.Add(comment);
            task.AddComment(comment);

            task.SetOpenStatus();
            task.RemoveEmployee();

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        private List<List<TaskDto>> GetNotNullLists(params List<TaskDto>[] list)
        {
            return list.Where(l => l.Count > 0).ToList();
        }
    }
}