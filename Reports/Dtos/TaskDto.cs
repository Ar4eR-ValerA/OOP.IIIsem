using System;
using System.Collections.Generic;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Dtos
{
    public class TaskDto
    {
        private readonly List<CommentDto> _comments;

        public TaskDto(
            string name, 
            string description, 
            Guid id, 
            List<CommentDto> comments, 
            string status, 
            BaseEmployeeDto employee,
            DateTime creationDate,
            DateTime lastChangeDate)
        {
            Name = name ?? throw new ReportsExceptions("Name is null");
            Description = description ?? throw new ReportsExceptions("Description is null");
            Id = id;
            _comments = comments ?? throw new ReportsExceptions("Comments are null");
            Status = status ?? throw new ReportsExceptions("Status is null");
            Employee = employee;
            CreationDate = creationDate;
            LastChangeDate = lastChangeDate;
        }
        
        public IReadOnlyList<CommentDto> Comments => _comments;
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }
        public BaseEmployeeDto Employee { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime LastChangeDate { get; private set; }

        public Task GetInstance()
        {
            return new Task(this);
        }
    }
}