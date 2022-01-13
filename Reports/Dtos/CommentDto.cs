using System;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Dtos
{
    public class CommentDto
    {
        public CommentDto()
        {
        }
        
        public CommentDto(Guid id, string name, string message, BaseEmployeeDto creator, DateTime creationDate)
        {
            Id = id;
            Name = name ?? throw new ReportsExceptions("Name is null");
            Message = message ?? throw new ReportsExceptions("Message is null");
            Creator = creator ?? throw new ReportsExceptions("Creator is null");
            CreationDate = creationDate;
        }
        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }
        public BaseEmployeeDto Creator { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public Comment GetInstance()
        {
            return new Comment(this);
        }
    }
}