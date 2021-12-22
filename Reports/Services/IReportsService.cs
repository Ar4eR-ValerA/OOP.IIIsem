using System;
using System.Collections.Generic;
using Reports.Dtos;

namespace Reports.Services
{
    public interface IReportsService
    {
        IReadOnlyList<ReportDto> FindByCreatorName(string creatorName);
        IReadOnlyList<ReportDto> FindByCreatorId(Guid creatorId);
        IReadOnlyList<ReportDto> FindById(Guid id);
        IReadOnlyList<ReportDto> FindByCreationDate(DateTime creationDate);
        IReadOnlyList<ReportDto> Find(string creatorName, Guid creatorId, Guid id, DateTime creationDate);
        IReadOnlyList<ReportDto> GetAllReports();
        void CreateEmployeeReport(Guid creatorId, DateTime creationDate, int sprintDays);
        void CreateTeamReport(Guid teamLeadId, DateTime creationDate, int sprintDays);
        void AddComment(
            Guid reportId, 
            Guid creatorId, 
            DateTime creationDate, 
            string commentName,
            string commentMessage);
    }
}