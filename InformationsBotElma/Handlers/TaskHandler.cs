using InformationsBotElma.Domain.Models;
using InformationsBotElma.Services.Interfaces;
using DomainTaskStatus = InformationsBotElma.Domain.Enums.TaskStatus;

namespace InformationsBotElma.Handlers;

public class TaskHandler
{
    private readonly ITaskService _taskService;

    public TaskHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public TaskModel CreateTask(string taskName, string description, DateTime deadline, string changedByUserName)
    {
        return _taskService.Create(taskName, description, deadline, changedByUserName);
    }

    public IReadOnlyCollection<TaskModel> GetAllTasks()
    {
        return _taskService.GetAll();
    }

    public bool UpdateTask(int taskId, string taskName, string description, DateTime deadline, DomainTaskStatus status, string changedByUserName)
    {
        return _taskService.Update(taskId, taskName, description, deadline, status, changedByUserName);
    }

    public bool DeleteTask(int taskId)
    {
        return _taskService.Delete(taskId);
    }
}
