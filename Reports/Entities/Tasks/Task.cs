using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Reports.Tools;

namespace Reports.Entities.Tasks
{
    public class Task
    {
        private readonly List<string> _comments;
        private BaseStatus _status;

        protected Task(string name, string description)
        {
            Name = name ?? throw new ReportsExceptions("Name is null");
            Description = description ?? throw new ReportsExceptions("Description is null");
            Id = Guid.NewGuid();
            _comments = new List<string>();
            Status = new OpenStatus();
        }

        public IReadOnlyList<string> Comments => _comments;
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public BaseStatus Status
        {
            get => _status;
            set => _status = value ?? throw new ReportsExceptions("Status is null");
        }

        public void AddComment(string comment)
        {
            if (comment is null)
            {
                throw new ReportsExceptions("Comment is null");
            }

            _comments.Add(comment);
        }

        public bool Equals(Task task)
        {
            return task is not null &&
                   Status == task.Status &&
                   Name == task.Name &&
                   Id == task.Id &&
                   Description == task.Description &&
                   !Comments.Except(task.Comments).ToList().Any() &&
                   !task.Comments.Except(Comments).ToList().Any();
        }
    }
}