using System;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Entities.Tasks
{
    public class Comment
    {
        public Comment()
        {
        }
        
        public Comment(string name, string message, BaseEmployee creator, DateTime creationDate)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ReportsExceptions("Name is null");
            Message = message ?? throw new ReportsExceptions("Message is null");
            Creator = creator ?? throw new ReportsExceptions("Creator is null");
            CreationDate = creationDate;
        }

        internal Comment(CommentDto commentDto)
        {
            if (commentDto is null)
            {
                throw new ReportsExceptions("CommentDto is null");
            }

            Id = commentDto.Id;
            Name = commentDto.Name;
            Message = commentDto.Message;
            Creator = commentDto.Creator.GetInstance();
            CreationDate = commentDto.CreationDate;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }
        public virtual BaseEmployee Creator { get; private set; }
        public DateTime CreationDate { get; private set; }

        public Comment GetCopy()
        {
            return new Comment(Name, Message, Creator, CreationDate);
        }

        public CommentDto GetDto()
        {
            return new CommentDto(Id, Name, Message, Creator.GetDto(), CreationDate);
        }
    }
}