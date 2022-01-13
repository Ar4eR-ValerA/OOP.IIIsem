using System;
using System.Collections.Generic;
using Reports.Entities.Reports;
using Reports.Tools;

namespace Reports.Dtos
{
    public class ReportDto
    {
        private readonly List<CommentDto> _comments;
        
        public ReportDto(List<CommentDto> comments, BaseEmployeeDto creator, DateTime creationDate, Guid id)
        {
            _comments = comments ?? throw new ReportsExceptions("Comments are null");
            Creator = creator ?? throw new ReportsExceptions("Creator is null");
            CreationDate = creationDate;
            Id = id;
        }
        
        public IReadOnlyList<CommentDto> Comments => _comments;
        public BaseEmployeeDto Creator { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Guid Id { get; private set; }

        public Report GetInstance()
        {
            return new Report(this);
        }
    }
}