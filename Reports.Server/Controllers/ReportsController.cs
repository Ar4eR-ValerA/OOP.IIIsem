using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.Dtos;
using Reports.Services;

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
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _reportsService.CreateEmployeeReport(creatorDto.Id, DateTime.Now, sprintDays);
        }

        [HttpPost]
        [Route("/reports/create-team-report")]
        public void CreateTeamReport(
            [FromQuery] string teamLeadName,
            [FromQuery] Guid teamLeadId,
            [FromQuery] int sprintDays)
        {
            BaseEmployeeDto teamLeadDto = _employeesService.FindOne(teamLeadName, teamLeadId);

            _reportsService.CreateTeamReport(teamLeadDto.Id, DateTime.Now, sprintDays);
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
            BaseEmployeeDto creatorDto = _employeesService.FindOne(creatorName, creatorId);

            _reportsService.AddComment(reportId, creatorDto.Id, DateTime.Now, commentName, commentMessage);
        }
    }
}