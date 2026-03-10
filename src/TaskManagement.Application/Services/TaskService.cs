using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;
        private readonly ITaskValidator validator;

        public TaskService(ITaskRepository repository, ITaskValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<Result<TaskResponse>> CreateTask(TaskEntity task)
        {
            var validationResult = validator.Validate(task);
            if (!validationResult.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(validationResult.Error);

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

        public async Task<Result<IEnumerable<TaskResponse>>> ListTask(string? status, DateTime? dueDate)
        {
            var result = await repository.List(status, dueDate);

            if (!result.IsSuccess)
                return Result<IEnumerable<TaskResponse>>.Failure<IEnumerable<TaskResponse>>(result.Error);
            if (!result.Data!.Any())
                return Result<IEnumerable<TaskResponse>>.Failure<IEnumerable<TaskResponse>>(TaskErrors.NotFound());

            var responseList = new List<TaskResponse>();
            foreach (var task in result.Data!)
            {
                responseList.Add(new TaskResponse
                {
                    Id = task.Id,
                    Titulo = task.Title,
                    Descricao = task.Description,
                    DataVencimento = task.DueDate,
                    Status = task.Status
                });
            }

            return Result<IEnumerable<TaskResponse>>.Success<IEnumerable<TaskResponse>>(responseList);
        }

        public async Task<Result<TaskResponse>> UpdateTask(TaskEntity taskEntity)
        {
            var result = await repository.Update(taskEntity);
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

            return Result<TaskResponse>.Success<TaskResponse>(response);
        }
    }
}
