using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;

        public TaskService(ITaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<TaskResponse>> CreateTask(TaskEntity task)
        {
            var result = await repository.Add(task);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);

            var response = new TaskResponse
            {
                Id = result.Data!.Id,
                Titulo = result.Data.Title,
                Descricao = result.Data.Description,
                DataVencimento = result.Data.DueDate,
                Status = result.Data.Status
            };

            return Result<TaskEntity>.Success(response);
        }

        public async Task<Result<TaskResponse>> DeleteTask(int id)
        {
            var result = await repository.Delete(id);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);
            if (result.Data == null)
                return Result<TaskResponse>.Failure<TaskResponse>(TaskErrors.NotFound());

            var response = new TaskResponse
            {
                Id = result.Data.Id,
                Titulo = result.Data.Title,
                Descricao = result.Data.Description,
                DataVencimento = result.Data.DueDate,
                Status = result.Data.Status
            };

            return Result<TaskResponse>.Success(response);
        }

        public async Task<Result<TaskResponse>> GetTask(int id)
        {
            var result = await repository.Get(id);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);
            if (result.Data == null)
                return Result<TaskResponse>.Failure<TaskResponse>(TaskErrors.NotFound());

            var response = new TaskResponse
            {
                Id = result.Data.Id,
                Titulo = result.Data.Title,
                Descricao = result.Data.Description,
                DataVencimento = result.Data.DueDate,
                Status = result.Data.Status
            };

            return Result<TaskResponse>.Success(response);
        }
    }
}
