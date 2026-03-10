using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Result;

namespace TaskManagement.Domain.Entities.Task
{
    public static class TaskErrors
    {
        public static Error NotFound() => new("Task.NotFound", ErrorType.NotFound);
        public static Error Unexpected(string message) => new(message, ErrorType.Unexpected);
    }
}
