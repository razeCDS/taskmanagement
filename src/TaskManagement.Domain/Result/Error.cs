using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using TaskManagement.Domain.Enum;

namespace TaskManagement.Domain.Result
{
    [SwaggerSchema("Representa um erro na aplicação")]
    public class Error
    {
        [SwaggerSchema("Mensagem descritiva do erro")]
        public string Message { get; }

        [SwaggerSchema("Tipo do erro (0-None, 1-NotFound, 2-Validation, 3-Unexpected)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
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
