using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Database;
using Reports.Dtos;
using Reports.Entities.Employees;
using Reports.Entities.Reports;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ReportsDatabaseContext _context;

        public ReportsService(ReportsDatabaseContext context)
        {
            _context = context;
        }

        public IReadOnlyList<ReportDto> FindByCreatorName(string creatorName)
        {
            return _context.Reports
                .Where(r => r.Creator.Name == creatorName)
                .Select(r => r.GetDto())
                .ToList();
        }

        public IReadOnlyList<ReportDto> FindByCreatorId(Guid creatorId)
        {
            return _context.Reports
                .Where(r => r.Creator.Id == creatorId)
                .Select(r => r.GetDto())
                .ToList();
        }

        public IReadOnlyList<ReportDto> FindById(Guid id)
        {
            return _context.Reports
                .Where(r => r.Id == id)
                .Select(r => r.GetDto())
                .ToList();
        }

        public IReadOnlyList<ReportDto> FindByCreationDate(DateTime creationDate)
        {
            return _context.Reports
                .Where(r => r.CreationDate == creationDate)
                .Select(r => r.GetDto())
                .ToList();
        }

        public IReadOnlyList<ReportDto> Find(string creatorName, Guid creatorId, Guid id, DateTime creationDate)
        {
            if (creatorName is null && creatorId == Guid.Empty && id == Guid.Empty && creationDate == default)
            {
                return GetAllReports();
            }

            var creatorNameReportDtos = new List<ReportDto>();
            if (!string.IsNullOrWhiteSpace(creatorName))
            {
                IReadOnlyList<ReportDto> result = FindByCreatorName(creatorName);
                creatorNameReportDtos.AddRange(result);
            }

            var creatorIdTaskDtos = new List<ReportDto>();
            if (creatorId != Guid.Empty)
            {
                IReadOnlyList<ReportDto> result = FindByCreatorId(creatorId);
                creatorIdTaskDtos.AddRange(result);
            }

            var idTaskDtos = new List<ReportDto>();
            if (id != Guid.Empty)
            {
                IReadOnlyList<ReportDto> result = FindById(id);
                idTaskDtos.AddRange(result);
            }

            var creationDateTaskDtos = new List<ReportDto>();
            if (creationDate != default)
            {
                IReadOnlyList<ReportDto> result = FindByCreationDate(creationDate);
                creationDateTaskDtos.AddRange(result);
            }

            List<List<ReportDto>> notNullLists = GetNotNullLists(
                creatorNameReportDtos,
                creatorIdTaskDtos,
                idTaskDtos,
                creationDateTaskDtos);

            if (notNullLists.Count == 0)
            {
                return new List<ReportDto>();
            }

            List<ReportDto> commonDtos = notNullLists.First();

            return notNullLists
                .Aggregate(commonDtos, (current, reportDtos)
                    => current.Intersect(reportDtos).ToList());
        }

        public ReportDto FindOne(string creatorName, Guid creatorId, Guid id, DateTime creationDate)
        {
            IReadOnlyList<ReportDto> reports = Find(creatorName, creatorId, id, creationDate);

            if (reports.Count == 0)
            {
                throw new ReportsExceptions("There are no suitable reports");
            }

            if (reports.Count > 1)
            {
                throw new ReportsExceptions("There are more then 1 suitable reports");
            }
            
            return reports.Single();
        }

        public IReadOnlyList<ReportDto> GetAllReports()
        {
            return _context.Reports.Select(r => r.GetDto()).ToList();
        }

        public void CreateEmployeeReport(Guid creatorId, DateTime creationDate, int sprintDays)
        {
            if (sprintDays <= 0)
            {
                throw new ReportsExceptions("Days of sprint must be positive");
            }

            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (creator is null || !creator.Active)
            {
                throw new ReportsExceptions("There is no such employee");
            }

            var comments = new List<Comment>();

            foreach (Task task in _context.Tasks)
            {
                var tempComments =
                    task.Comments
                        .Where(c => c.CreationDate > creationDate - GetDaySpan(sprintDays))
                        .Where(c => c.Creator == creator)
                        .Select(c => c.GetCopy())
                        .ToList();

                _context.Comments.AddRange(tempComments);
                comments.AddRange(tempComments);
            }

            var report = new Report(
                comments,
                creator,
                creationDate);

            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public void CreateTeamReport(Guid teamLeadId, DateTime creationDate, int sprintDays)
        {
            if (sprintDays <= 0)
            {
                throw new ReportsExceptions("Days of sprint must be positive");
            }

            BaseEmployee creator = _context.BaseEmployees.Find(teamLeadId);

            if (creator is null || !creator.Active)
            {
                throw new ReportsExceptions("There is no such teamLead");
            }

            if (creator is not TeamLead teamLead)
            {
                throw new ReportsExceptions("There is no such teamLead");
            }

            var comments = new List<Comment>();

            foreach (Report report in _context.Reports
                .Where(r => r.CreationDate > creationDate - GetDaySpan(sprintDays))
                .Where(r => teamLead.Subordinates.Contains(r.Creator) || r.Creator == teamLead))
            {
                IReadOnlyList<Comment> tempComments = report.Comments.Select(c => c.GetCopy()).ToList();
                _context.Comments.AddRange(tempComments);
                comments.AddRange(tempComments);
            }

            var teamReport = new Report(
                comments,
                creator,
                creationDate);

            _context.Reports.Add(teamReport);
            _context.SaveChanges();
        }

        public void AddComment(
            Guid reportId,
            Guid creatorId,
            DateTime creationDate,
            string commentName,
            string commentMessage)
        {
            Report report = _context.Reports.Find(reportId);
            BaseEmployee creator = _context.BaseEmployees.Find(creatorId);

            if (report is null)
            {
                throw new ReportsExceptions("There is no such report");
            }

            if (creator is null || !creator.Active)
            {
                throw new ReportsExceptions("There is no such employee");
            }

            var comment = new Comment(commentName, commentMessage, creator, creationDate);
            _context.Comments.Add(comment);
            report.AddComment(comment);

            _context.Reports.Update(report);
            _context.SaveChanges();
        }


        private TimeSpan GetDaySpan(int days)
        {
            return new TimeSpan(days, 0, 0, 0);
        }

        private List<List<ReportDto>> GetNotNullLists(params List<ReportDto>[] list)
        {
            return list.Where(l => l.Count > 0).ToList();
        }
    }
}