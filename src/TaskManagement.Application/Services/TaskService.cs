using Microsoft.Extensions.Logging;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;
        private readonly ITaskValidator validator;
        private readonly ILogger<TaskService> logger;

        public TaskService(ITaskRepository repository, ITaskValidator validator, ILogger<TaskService> logger)
        {
            this.repository = repository;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<Result<TaskResponse>> CreateTask(TaskRequest task)
        {
            logger.LogInformation("Iniciando criação de tarefa. Titulo: {Titulo}", task.Titulo);

            var validationResult = validator.Validate(task);
            if (!validationResult.IsSuccess)
            {
                logger.LogWarning("Falha na validação da tarefa. Titulo: {Titulo}", task.Titulo);
                return Result.Failure<TaskResponse>(validationResult.Error);
            }

            var taskEntity = TaskMapper.MapToEntity(task);
            var result = await repository.Add(taskEntity);

            if (!result.IsSuccess)
            {
                logger.LogError("Erro ao persistir tarefa. Titulo: {Titulo}", task.Titulo);
                return Result.Failure<TaskResponse>(result.Error);
            }

            logger.LogInformation("Tarefa criada com sucesso. TaskId: {TaskId}", result.Data!.Id);

            var response = TaskMapper.MapToResponse(result.Data);
            return Result.Success(response);
        }

        public async Task<Result<TaskResponse>> DeleteTask(int id)
        {
            logger.LogInformation("Removendo tarefa. TaskId: {TaskId}", id);

            var result = await repository.Delete(id);

            if (!result.IsSuccess)
            {
                logger.LogError("Erro ao remover tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(result.Error);
            }

            if (result.Data == null)
            {
                logger.LogWarning("Tentativa de remover tarefa inexistente. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(TaskErrors.NotFound());
            }

            logger.LogInformation("Tarefa removida com sucesso. TaskId: {TaskId}", id);

            var response = TaskMapper.MapToResponse(result.Data);
            return Result.Success(response);
        }

        public async Task<Result<TaskResponse>> GetTask(int id)
        {
            logger.LogInformation("Buscando tarefa. TaskId: {TaskId}", id);

            var result = await repository.Get(id);

            if (!result.IsSuccess)
            {
                logger.LogError("Erro ao buscar tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(result.Error);
            }

            if (result.Data == null)
            {
                logger.LogWarning("Tarefa não encontrada. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(TaskErrors.NotFound());
            }

            logger.LogInformation("Tarefa encontrada. TaskId: {TaskId}", id);

            var response = TaskMapper.MapToResponse(result.Data);
            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<TaskResponse>>> ListTask(string? status, DateTime? dueDate, int? page, int? pageSize)
        {
            logger.LogInformation("Listando tarefas. Status: {Status}, DueDate: {DueDate}", status, dueDate);

            var result = await repository.List(status, dueDate, page, pageSize);
            if (!result.IsSuccess)
            {
                logger.LogError("Erro ao listar tarefas. Status: {Status}, DueDate: {DueDate}", status, dueDate);
                return Result.Failure<IEnumerable<TaskResponse>>(result.Error);
            }

            if (!result.Data!.Any())
            {
                logger.LogInformation("Nenhuma tarefa encontrada para os filtros informados.");
                return Result.Failure<IEnumerable<TaskResponse>>(TaskErrors.NotFound());
            }

            logger.LogInformation("Tarefas listadas com sucesso. Quantidade: {Count}", result.Data.Count());

            var responseList = result.Data.Select(task => TaskMapper.MapToResponse(task));
            return Result.Success(responseList);
        }

        public async Task<Result<TaskResponse>> UpdateTask(int id, TaskRequest task)
        {
            logger.LogInformation("Atualizando tarefa. TaskId: {TaskId}", id);

            var validationResult = validator.Validate(task);
            if (!validationResult.IsSuccess)
            {
                logger.LogWarning("Falha na validação da atualização da tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(validationResult.Error);
            }

            var getTask = await GetTask(id);
            if (!getTask.IsSuccess)
            {
                logger.LogWarning("Tentativa de atualizar tarefa inexistente. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(getTask.Error);
            }

            var taskEntity = TaskMapper.MapToEntity(task);
            var result = await repository.Update(id, taskEntity);

            if (!result.IsSuccess)
            {
                logger.LogError("Erro ao atualizar tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskResponse>(result.Error);
            }

            logger.LogInformation("Tarefa atualizada com sucesso. TaskId: {TaskId}", id);

            var response = TaskMapper.MapToResponse(result.Data!);
            return Result.Success(response);
        }
    }
}
