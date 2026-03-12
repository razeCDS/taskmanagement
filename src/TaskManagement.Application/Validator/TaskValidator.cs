using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Validator
{
    public class TaskValidator : ITaskValidator
    {
        public Result Validate(TaskRequest task)
        {
            if (string.IsNullOrWhiteSpace(task.Titulo))
                return Result.Failure<TaskRequest>(TaskErrors.Validation("Título é obrigatório."));

            if (!Enum.TryParse<TaskStatusEnum>(task.Status, true, out var status))
                return Result.Failure<TaskRequest>(TaskErrors.Validation("Status inválido."));

            return Result.Success();
        }
    }
}
