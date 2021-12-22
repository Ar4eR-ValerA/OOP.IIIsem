using System;
using System.Collections.Generic;
using Reports.Entities.Employees;
using Reports.Tools;

namespace Reports.Dtos
{
    public class TeamLeadDto : BaseEmployeeDto
    {
        private readonly List<BaseEmployeeDto> _subordinates;

        public TeamLeadDto(Guid id, string name, List<BaseEmployeeDto> subordinates, bool active)
            : base(id, name ?? throw new ReportsExceptions("Name is null"), active)
        {
            _subordinates = subordinates ?? throw new ReportsExceptions("Subordinates are null");
        }
        
        public IReadOnlyList<BaseEmployeeDto> Subordinates => _subordinates;

        public override BaseEmployee GetInstance()
        {
            return new TeamLead(this);
        }
    }
}