using InformationsBotElma.Domain.Models;
using InformationsBotElma.Repositories.Interfaces;

namespace InformationsBotElma.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<int, TaskModel> _tasks = new();

    public TaskModel Add(TaskModel task)
    {
        _tasks[task.TaskId] = Copy(task);
        return Copy(task);
    }

    public IReadOnlyCollection<TaskModel> GetAll()
    {
        return _tasks.Values.OrderBy(task => task.TaskId).Select(Copy).ToArray();
    }

    public TaskModel? GetById(int taskId)
    {
        return _tasks.TryGetValue(taskId, out var task) ? Copy(task) : null;
    }

    public bool Update(TaskModel task)
    {
        if (!_tasks.ContainsKey(task.TaskId))
        {
            return false;
        }

        _tasks[task.TaskId] = Copy(task);
        return true;
    }

    public bool Delete(int taskId)
    {
        return _tasks.Remove(taskId);
    }

    private static TaskModel Copy(TaskModel task)
    {
        return new TaskModel
        {
            TaskId = task.TaskId,
            TaskName = task.TaskName,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            DeadLine = task.DeadLine,
            LastChangedAt = task.LastChangedAt,
            LastChangedByUserName = task.LastChangedByUserName,
            TaskStatus = task.TaskStatus
        };
    }
}
