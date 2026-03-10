using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Extensions;
using TaskManagement.Api.Requests.Task;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService service;

        public TaskController(ITaskService service)
        {
            this.service = service;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<TaskEntity>> Get([FromQuery] int id)
        {
            var result = await service.GetTask(id);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<TaskEntity>> Create([FromBody] CreateTaskRequest task)
        {
            var taskEntity = new TaskEntity
            {
                Title = task.Titulo!,
                Description = task.Descricao,
                DueDate = task.DataVencimento,
                Status = task.Status!
            };

            var result = await service.CreateTask(taskEntity);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return CreatedAtAction(nameof(Create), new { id = result.Data!.Id }, result);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<TaskEntity>> Delete([FromQuery] int id)
        {
            var result = await service.DeleteTask(id);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return NoContent();
        }

        [HttpGet("List")]
        public async Task<ActionResult<TaskEntity>> List([FromQuery] string? status, [FromQuery] DateTime? dataVencimento)
        {
            var result = await service.ListTask(status, dataVencimento);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<TaskEntity>> Update([FromBody] UpdateTaskRequest task)
        {
            var taskEntity = new TaskEntity
            {
                Id = task.Id,
                Title = task.Titulo!,
                Description = task.Descricao,
                DueDate = task.DataVencimento,
                Status = task.Status!
            };

            var result = await service.UpdateTask(taskEntity);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
