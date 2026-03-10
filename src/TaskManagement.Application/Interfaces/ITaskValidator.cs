using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskValidator
    {
        Result<TaskRequest> Validate(TaskRequest task);
    }
}