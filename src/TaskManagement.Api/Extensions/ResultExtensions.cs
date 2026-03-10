using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Result;

namespace TaskManagement.Api.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);

            return result.Error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(result),
                ErrorType.Validation => new BadRequestObjectResult(result),
                _ => new ObjectResult(result) { StatusCode = 500 }
            };
        }
    }
}
