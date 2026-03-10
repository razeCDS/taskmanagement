using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<Result<TaskResponse>> CreateTask(TaskEntity task);
        Task<Result<TaskResponse>> GetTask(int id);
        Task<Result<TaskResponse>> DeleteTask(int id);
        Task<Result<IEnumerable<TaskResponse>>> ListTask(string status, DateTime dueDate);
    }
}