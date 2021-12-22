using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.Dtos;
using Reports.Services;
using Reports.Tools;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/reports")]
    public class ReportsController
    {
        private readonly IReportsService _reportsService;
        private readonly IEmployeesService _employeesService;

        public ReportsController(IReportsService reportsService, IEmployeesService employeesService)
        {
            _reportsService = reportsService;
            _employeesService = employeesService;
        }

        [HttpPost]
        [Route("/reports/create-employee-report")]
        public void CreateEmployeeReport(
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId,
            [FromQuery] int sprintDays)
        {
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(creatorDtos);

            _reportsService.CreateEmployeeReport(creatorDtos.First().Id, DateTime.Now, sprintDays);
        }

        [HttpPost]
        [Route("/reports/create-team-report")]
        public void CreateTeamReport(
            [FromQuery] string teamLeadName,
            [FromQuery] Guid teamLeadId,
            [FromQuery] int sprintDays)
        {
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(teamLeadName, teamLeadId);
            IsOne(creatorDtos);

            _reportsService.CreateTeamReport(teamLeadId, DateTime.Now, sprintDays);
        }

        [HttpGet]
        [Route("/reports/find")]
        public IReadOnlyList<ReportDto> Find(
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId,
            [FromQuery] Guid id,
            [FromQuery] DateTime creationDate)
        {
            return _reportsService.Find(creatorName, creatorId, id, creationDate);
        }

        [HttpPatch]
        [Route("/reports/add-comment")]
        public void AddComment(
            [FromQuery] Guid reportId,
            [FromQuery] string creatorName,
            [FromQuery] Guid creatorId,
            [FromQuery] string commentName,
            [FromQuery] string commentMessage)
        {
            IReadOnlyList<BaseEmployeeDto> creatorDtos = _employeesService.Find(creatorName, creatorId);
            IsOne(creatorDtos);

            _reportsService.AddComment(reportId, creatorDtos.First().Id, DateTime.Now, commentName, commentMessage);
        }

        private void IsOne(IReadOnlyList<object> objects)
        {
            if (objects is null)
            {
                throw new ReportsExceptions("There is no objects");
            }

            if (objects.Count == 0)
            {
                throw new ReportsExceptions(
                    $"There is not such {objects.GetType().GetGenericArguments().First().Name}");
            }

            if (objects.Count > 1)
            {
                throw new ReportsExceptions($"There are more than 1 suitable {objects.First().GetType().Name}");
            }
        }
    }
}