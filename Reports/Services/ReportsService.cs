using System.Collections.Generic;
using System.Linq;
using Reports.Entities.Employees;
using Reports.Entities.Reports;
using Reports.Entities.Tasks;
using Reports.Tools;

namespace Reports.Services
{
    public class ReportsService
    {
        public EmployeeReport CreateEmployeeReport(EmployeeReport previousReport, BaseEmployee creator)
        {
            if (previousReport is null)
            {
                throw new ReportsExceptions("Previous report is null");
            }

            IReadOnlyList<Task> actualTasks = SyncService.GetActualTasks();
            IReadOnlyList<Task> previousReportTasks = previousReport.Tasks;
            var newTasks = new List<Task>();

            foreach (Task actualTask in actualTasks)
            {
                Task previousReportTask = previousReportTasks.SingleOrDefault(t => t.Id == actualTask.Id);

                if (actualTask.Equals(previousReportTask))
                {
                    newTasks.Add(actualTask);
                }

                if (actualTask.Status.GetType() == typeof(ResolvedStatus))
                {
                    SyncService.TransferTaskToOutDated(actualTask);
                }
            }
            
            // TODO: Закинуть на сервер новый репорт
            return new EmployeeReport(newTasks, creator);
        }
    }
}