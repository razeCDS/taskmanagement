using TaskManagement.Domain.Enum;

namespace TaskManagement.Domain.Result
{
    public class Error
    {
        public string Message { get; }
        public ErrorType Type { get; }

        public Error(string message, ErrorType type)
        {
            Message = message;
            Type = type;
        }

        public static Error None => new(string.Empty, ErrorType.None);
        public static Error NotFound(string message) => new(message, ErrorType.NotFound);
        public static Error Validation(string message) => new(message, ErrorType.Validation);
        public static Error Unexpected(string message) => new(message, ErrorType.Unexpected);

        public static implicit operator Error(string message) => new(message, ErrorType.Unexpected);
    }
}
