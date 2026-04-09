using InformationsBotElma.Domain.Models;

namespace InformationsBotElma.Repositories.Interfaces;

public interface ITaskRepository
{
    TaskModel Add(TaskModel task);
    IReadOnlyCollection<TaskModel> GetAll();
    TaskModel? GetById(int taskId);
    bool Update(TaskModel task);
    bool Delete(int taskId);
}
