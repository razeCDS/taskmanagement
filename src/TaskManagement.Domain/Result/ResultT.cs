using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Result
{
    [SwaggerSchema("Resultado da API com dados")]
    public class Result<T> : Result
    {
        [SwaggerSchema("Dados retornados (null quando IsSuccess = false)")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; }

        public Result(bool isSuccess, Error error, T? data) : base(isSuccess, error)
        {
            Data = data;
        }
    }
}
