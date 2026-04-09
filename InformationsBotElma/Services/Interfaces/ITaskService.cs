using InformationsBotElma.Domain.Models;
using DomainTaskStatus = InformationsBotElma.Domain.Enums.TaskStatus;

namespace InformationsBotElma.Services.Interfaces;

public interface ITaskService
{
    TaskModel Create(string taskName, string description, DateTime deadline, string changedByUserName);
    IReadOnlyCollection<TaskModel> GetAll();
    TaskModel? GetById(int taskId);
    bool Update(int taskId, string taskName, string description, DateTime deadline, DomainTaskStatus status, string changedByUserName);
    bool Delete(int taskId);
}
