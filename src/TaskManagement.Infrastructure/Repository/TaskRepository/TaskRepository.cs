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
                var task = await context.Task.Where(t => t.Id == id).FirstOrDefaultAsync();
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
                var result = await context.Task.Where(a => a.Id == id).FirstOrDefaultAsync();
                return Result<TaskEntity>.Success(result!);
            }
            catch (Exception e)
            {
                return Result<TaskEntity>.Failure<TaskEntity>(TaskErrors.Unexpected(e.Message));
            }
        }
    }
}
