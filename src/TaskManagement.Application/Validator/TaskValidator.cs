using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Validator
{
    public class TaskValidator : ITaskValidator
    {
        public Result<TaskEntity> Validate(TaskEntity task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return Result.Failure<TaskEntity>(TaskErrors.Validation("Título é obrigatório."));

            if (!Enum.TryParse<TaskStatusEnum>(task.Status, true, out var status))
                return Result.Failure<TaskEntity>(TaskErrors.Validation("Status inválido."));

            return Result.Success(task);
        }
    }
}
