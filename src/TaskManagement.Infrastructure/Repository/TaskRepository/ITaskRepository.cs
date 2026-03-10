using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Result<TaskEntity>> Add(TaskEntity task);
        Task<Result<TaskEntity>> Delete(int id);
        Task<Result<TaskEntity>> Get(int id);
    }
}
