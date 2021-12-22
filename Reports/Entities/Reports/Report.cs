using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Entities.Reports
{
    public class Report
    {
        private readonly List<Comment> _comments;

        public Report(List<Comment> comments, BaseEmployee creator, DateTime creationDate)
        {
            _comments = comments ?? throw new ReportsExceptions("Tasks are null");
            Creator = creator ?? throw new ReportsExceptions("Creator is null");
            CreationDate = creationDate;
            Id = Guid.NewGuid();
        }

        public Report()
        {
            _comments = new List<Comment>();
        }

        internal Report(ReportDto reportDto)
        {
            if (reportDto is null)
            {
                throw new ReportsExceptions("ReportDto is null");
            }

            _comments = reportDto.Comments.Select(c => c.GetInstance()).ToList();
            CreationDate = reportDto.CreationDate;
            Id = reportDto.Id;

            if (reportDto.Creator is ManagerDto managerDto)
            {
                Creator = new Manager(managerDto);
            }

            if (reportDto.Creator is EmployeeDto employeeDto)
            {
                Creator = new Employee(employeeDto);
            }

            if (reportDto.Creator is TeamLeadDto teamLeadDto)
            {
                Creator = new TeamLead(teamLeadDto);
            }
        }

        public virtual IReadOnlyList<Comment> Comments => _comments;
        public virtual BaseEmployee Creator { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Guid Id { get; private set; }

        public void AddComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ReportsExceptions("Comment is null");
            }

            _comments.Add(comment);
        }

        public ReportDto GetDto()
        {
            return new ReportDto(Comments.Select(c => c.GetDto()).ToList(), Creator.GetDto(), CreationDate, Id);
        }
    }
}