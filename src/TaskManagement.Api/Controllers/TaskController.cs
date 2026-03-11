using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Extensions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Result;

namespace TaskManagement.Api.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService service;

        public TaskController(ITaskService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Obtém a task pelo id.")]
        [ProducesResponseType(typeof(Result<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponse>> Get(int id)
        {
            var result = await service.GetTask(id);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPost]
        [EndpointSummary("Cria uma nova Task.")]
        [ProducesResponseType(typeof(Result<TaskResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponse>> Create([FromBody] TaskRequest request)
        {
            var result = await service.CreateTask(request);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return CreatedAtAction(nameof(Create), new { id = result.Data!.Id }, result);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Deleta uma task.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await service.DeleteTask(id);
            if (!result.IsSuccess)
                return ResultExtensions.ToActionResult(result);

            return NoContent();
        }

        [HttpGet("list")]
        [EndpointSummary("Realiza a listagem de tasks, por filtro de status e data.")]
        [ProducesResponseType(typeof(Result<IEnumerable<TaskResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskResponse>> List([FromQuery] string? status, [FromQuery] DateTime? dataVencimento, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var result = await service.ListTask(status, dataVencimento, page, pageSize);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Realiza a atualização de uma task.")]
        [ProducesResponseType(typeof(Result<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponse>> Update(int id, [FromBody] TaskRequest request)
        {
            var result = await service.UpdateTask(id, request);
            return ResultExtensions.ToActionResult(result);
        }
    }
}
