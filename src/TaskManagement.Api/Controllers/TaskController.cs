using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Extensions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Requests.Task;
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
        public async Task<ActionResult<TaskResponse>> Get([FromQuery] int id)
        {
            var result = await service.GetTask(id);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest task)
        {
            var result = await service.CreateTask(task);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return CreatedAtAction(nameof(Create), new { id = result.Data!.Id }, result);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<TaskResponse>> Delete([FromQuery] int id)
        {
            var result = await service.DeleteTask(id);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return NoContent();
        }

        [HttpGet("List")]
        public async Task<ActionResult<TaskResponse>> List([FromQuery] string? status, [FromQuery] DateTime? dataVencimento)
        {
            var result = await service.ListTask(status, dataVencimento);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<TaskResponse>> Update([FromBody] UpdateTaskRequest task)
        {
            var result = await service.UpdateTask(task);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
