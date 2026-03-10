using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskValidator
    {
        Result<TaskEntity> Validate(TaskEntity task);
    }
}