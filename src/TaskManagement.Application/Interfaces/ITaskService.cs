using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<Result<TaskResponse>> CreateTask(CreateTaskRequest task);
        Task<Result<TaskResponse>> GetTask(int id);
        Task<Result<TaskResponse>> DeleteTask(int id);
        Task<Result<IEnumerable<TaskResponse>>> ListTask(string? status, DateTime? dueDate);
        Task<Result<TaskResponse>> UpdateTask(int id, UpdateTaskRequest task);
    }
}