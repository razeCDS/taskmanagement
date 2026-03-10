using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repository.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext context;
        private readonly ILogger<TaskRepository> logger;

        public TaskRepository(TaskDbContext context, ILogger<TaskRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Result<TaskEntity>> Add(TaskEntity task)
        {
            try
            {
                logger.LogInformation("Persistindo nova tarefa no banco. Titulo: {Titulo}", task.Title);

                var result = await context.Task.AddAsync(task);
                await context.SaveChangesAsync();

                logger.LogInformation("Tarefa persistida com sucesso. TaskId: {TaskId}", result.Entity.Id);

                return Result.Success(result.Entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao inserir tarefa no banco. Titulo: {Titulo}", task.Title);
                return Result.Failure<TaskEntity>(TaskErrors.Unexpected(ex.Message));
            }
        }

        public async Task<Result<TaskEntity>> Delete(int id)
        {
            try
            {
                logger.LogInformation("Removendo tarefa do banco. TaskId: {TaskId}", id);

                var task = await context.Task.AsNoTracking()
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                if (task == null)
                {
                    logger.LogWarning("Tentativa de remover tarefa inexistente. TaskId: {TaskId}", id);
                    return Result.Failure<TaskEntity>(TaskErrors.NotFound());
                }

                context.Task.Remove(task);
                await context.SaveChangesAsync();

                logger.LogInformation("Tarefa removida com sucesso. TaskId: {TaskId}", id);

                return Result.Success(task);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao remover tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskEntity>(TaskErrors.Unexpected(ex.Message));
            }
        }

        public async Task<Result<TaskEntity>> Get(int id)
        {
            try
            {
                logger.LogInformation("Buscando tarefa no banco. TaskId: {TaskId}", id);

                var result = await context.Task.AsNoTracking()
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                    logger.LogWarning("Tarefa não encontrada no banco. TaskId: {TaskId}", id);

                return Result.Success(result!);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskEntity>(TaskErrors.Unexpected(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<TaskEntity>>> List(string? status, DateTime? dueDate)
        {
            try
            {
                logger.LogInformation("Listando tarefas. Status: {Status}, DueDate: {DueDate}", status, dueDate);

                var result = await context.Task.AsNoTracking()
                    .Where(t => (status == null || t.Status == status) &&
                                (!dueDate.HasValue || t.DueDate <= dueDate))
                    .ToListAsync();

                logger.LogInformation("Consulta de tarefas executada. Quantidade encontrada: {Count}", result.Count);

                return Result.Success<IEnumerable<TaskEntity>>(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao listar tarefas. Status: {Status}, DueDate: {DueDate}", status, dueDate);
                return Result.Failure<IEnumerable<TaskEntity>>(TaskErrors.Unexpected(ex.Message));
            }
        }

        public async Task<Result<TaskEntity>> Update(int id, TaskEntity taskEntity)
        {
            try
            {
                logger.LogInformation("Atualizando tarefa no banco. TaskId: {TaskId}", id);

                var task = await context.Task.AsNoTracking()
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                if (task == null)
                {
                    logger.LogWarning("Tentativa de atualizar tarefa inexistente. TaskId: {TaskId}", id);
                    return Result.Failure<TaskEntity>(TaskErrors.NotFound());
                }

                taskEntity.Id = id;
                var result = context.Task.Update(taskEntity);

                await context.SaveChangesAsync();

                logger.LogInformation("Tarefa atualizada com sucesso. TaskId: {TaskId}", id);

                return Result.Success(result.Entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar tarefa. TaskId: {TaskId}", id);
                return Result.Failure<TaskEntity>(TaskErrors.Unexpected(ex.Message));
            }
        }
    }
}
