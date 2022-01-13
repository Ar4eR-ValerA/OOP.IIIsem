using System;
using System.Collections.Generic;
using Reports.Dtos;

namespace Reports.Services
{
    public interface ITasksService
    {
        void CreateTask(string name, string description, Guid creatorId, DateTime creationDate);
        IReadOnlyList<TaskDto> FindByName(string name);
        IReadOnlyList<TaskDto> FindById(Guid id);
        IReadOnlyList<TaskDto> FindByAssignedId(Guid assignedId);
        IReadOnlyList<TaskDto> FindByCreationDate(DateTime creationDate);
        IReadOnlyList<TaskDto> FindByLastChangeDate(DateTime lastChangeDate);

        IReadOnlyList<TaskDto> Find(string name, Guid id);

        IReadOnlyList<TaskDto> Find(
            string name,
            Guid id,
            Guid assignedId,
            DateTime creationDate,
            DateTime lastChangeDate);

        TaskDto FindOne(string name, Guid id);
        IReadOnlyList<TaskDto> GetAllTasks();

        void AddComment(
            Guid taskId,
            string commentName,
            string commentMessage,
            Guid creatorId,
            DateTime creationDate);

        void ChangeDescription(Guid taskId, string description);
        void AssignEmployee(Guid taskId, Guid employeeId, Guid creatorId, DateTime assignDate);
        void FinishTask(Guid taskId, Guid creatorId, DateTime finishDate);
        void OpenTask(Guid taskId, Guid creatorId, DateTime finishDate);
    }
}