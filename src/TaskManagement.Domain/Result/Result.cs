using Swashbuckle.AspNetCore.Annotations;

namespace TaskManagement.Domain.Result
{
    [SwaggerSchema("Resultado base da API")]
    public class Result
    {
        [SwaggerSchema("Indica se a operação foi bem-sucedida")]
        public bool IsSuccess { get; }

        [SwaggerSchema("Detalhes do erro")]
        public Error Error { get; }

        public Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<T> Success<T>(T data) => new(true, Error.None, data);
        public static Result<T> Failure<T>(Error error) => new(false, error, default);
    }
}

