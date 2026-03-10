using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repository.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext context;

        public TaskRepository(TaskDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<TaskEntity>> Add(TaskEntity task)
        {
            try
            {
                var result = await context.Task.AddAsync(task);
                await context.SaveChangesAsync();
                return Result<TaskEntity>.Success(result.Entity);
            }
            catch (Exception e)
            {
                return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.Unexpected(e.Message));
            }
        }

        public async Task<Result<TaskEntity>> Delete(int id)
        {
            try
            {
                var task = await context.Task.AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
                if (task == null)
                    return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.NotFound());
                
                context.Task.Remove(task);
                await context.SaveChangesAsync();
                return Result<TaskEntity>.Success(task);
            }
            catch (Exception e)
            {
                return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.Unexpected(e.Message));
            }
        }

        public async Task<Result<TaskEntity>> Get(int id)
        {
            try
            {
                var result = await context.Task.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
                return Result<TaskEntity>.Success(result!);
            }
            catch (Exception e)
            {
                return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.Unexpected(e.Message));
            }
        }

        public async Task<Result<IEnumerable<TaskEntity>>> List(string? status, DateTime? dueDate)
        {
            try
            {
                var result = await context.Task.AsNoTracking().Where(t => (status == null || t.Status == status) && (!dueDate.HasValue || t.DueDate <= dueDate)).ToListAsync();
                return Result<IEnumerable<TaskEntity>>.Success<IEnumerable<TaskEntity>>(result);
            }
            catch(Exception e)
            {
                return Result<IEnumerable<TaskEntity>>.Failure<IEnumerable<TaskEntity>>(TaskErrors.Unexpected(e.Message));
            }
        }

        public async Task<Result<TaskEntity>> Update(int id, TaskEntity taskEntity)
        {
            try
            {
                var task = await context.Task.AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync();
                if (task == null)
                    return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.NotFound());

                taskEntity.Id = id;
                var result = context.Task.Update(taskEntity);
                await context.SaveChangesAsync();
                return Result<TaskEntity>.Success(result.Entity);

            }
            catch (Exception e) 
            {
                return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.Unexpected(e.Message));
            }
        }
    }
}
