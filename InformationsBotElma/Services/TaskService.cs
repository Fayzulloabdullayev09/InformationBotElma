using InformationsBotElma.Domain.Models;
using InformationsBotElma.Repositories.Interfaces;
using InformationsBotElma.Services.Interfaces;
using DomainTaskStatus = InformationsBotElma.Domain.Enums.TaskStatus;

namespace InformationsBotElma.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private int _nextTaskId = 1;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public TaskModel Create(string taskName, string description, DateTime deadline, string changedByUserName)
    {
        Validate(taskName, description, deadline, changedByUserName);

        var now = DateTime.UtcNow;
        var task = new TaskModel
        {
            TaskId = _nextTaskId++,
            TaskName = taskName.Trim(),
            Description = description.Trim(),
            CreatedAt = now,
            DeadLine = deadline,
            LastChangedAt = now,
            LastChangedByUserName = changedByUserName.Trim(),
            TaskStatus = DomainTaskStatus.Planned
        };

        return _taskRepository.Add(task);
    }

    public IReadOnlyCollection<TaskModel> GetAll()
    {
        return _taskRepository.GetAll();
    }

    public TaskModel? GetById(int taskId)
    {
        return _taskRepository.GetById(taskId);
    }

    public bool Update(int taskId, string taskName, string description, DateTime deadline, DomainTaskStatus status, string changedByUserName)
    {
        Validate(taskName, description, deadline, changedByUserName);

        var task = _taskRepository.GetById(taskId);
        if (task is null)
        {
            return false;
        }

        task.TaskName = taskName.Trim();
        task.Description = description.Trim();
        task.DeadLine = deadline;
        task.TaskStatus = status;
        task.LastChangedByUserName = changedByUserName.Trim();
        task.LastChangedAt = DateTime.UtcNow;
        return _taskRepository.Update(task);
    }

    public bool Delete(int taskId)
    {
        return _taskRepository.Delete(taskId);
    }

    private static void Validate(string taskName, string description, DateTime deadline, string changedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskName);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(changedByUserName);

        if (deadline <= DateTime.UtcNow)
        {
            throw new ArgumentException("Deadline hozirgi vaqtdan keyin bo'lishi kerak.", nameof(deadline));
        }
    }
}
