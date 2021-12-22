using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Entities.Tasks
{
    public class Task
    {
        private readonly List<Comment> _comments;

        public Task(string name, string description, DateTime creationDate)
        {
            Name = name ?? throw new ReportsExceptions("Name is null");
            Description = description ?? throw new ReportsExceptions("Description is null");
            Id = Guid.NewGuid();
            _comments = new List<Comment>();
            Status = "Open";
            CreationDate = creationDate;
            LastChangeDate = creationDate;
        }

        internal Task(TaskDto taskDto)
        {
            if (taskDto is null)
            {
                throw new ReportsExceptions("TaskDto is null");
            }

            Name = taskDto.Name;
            Description = taskDto.Description;
            Id = taskDto.Id;
            _comments = taskDto.Comments.Select(c => c.GetInstance()).ToList();
            Status = taskDto.Status;
            CreationDate = taskDto.CreationDate;
            LastChangeDate = taskDto.LastChangeDate;

            if (taskDto.Employee is ManagerDto managerDto)
            {
                Employee = new Manager(managerDto);
            }

            if (taskDto.Employee is EmployeeDto employeeDto)
            {
                Employee = new Employee(employeeDto);
            }

            if (taskDto.Employee is TeamLeadDto teamLeadDto)
            {
                Employee = new TeamLead(teamLeadDto);
            }
        }

        public virtual IReadOnlyList<Comment> Comments => _comments;
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime LastChangeDate { get; private set; }

        public virtual BaseEmployee Employee { get; private set; }

        public void ChangeDescription(string description)
        {
            Description = description ?? throw new ReportsExceptions("Description is null");
        }

        public void AssignEmployee(BaseEmployee employee)
        {
            Employee = employee;
        }
        
        public void RemoveEmployee()
        {
            if (Employee is null)
            {
                return;
            }

            Employee = null;
        }

        public void AddComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ReportsExceptions("Comment is null");
            }

            _comments.Add(comment);
            LastChangeDate = comment.CreationDate;
        }

        public void SetOpenStatus()
        {
            Status = "Open";
        }
        
        public void SetActiveStatus()
        {
            Status = "Active";
        }
        
        public void SetResolvedStatus()
        {
            Status = "Resolved";
        }
        
        public TaskDto GetDto()
        {
            return new TaskDto(
                Name,
                Description,
                Id,
                _comments.Select(c => c.GetDto()).ToList(),
                Status,
                Employee?.GetDto(),
                CreationDate,
                LastChangeDate);
        }
    }
}