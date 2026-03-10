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

        public TaskService(ITaskRepository repository, ITaskValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<Result<TaskResponse>> CreateTask(CreateTaskRequest task)
        {
            var taskEntity = TaskMapper.MapToEntity(task);

            var validationResult = validator.Validate(taskEntity);
            if (!validationResult.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(validationResult.Error);

            var result = await repository.Add(taskEntity);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);

            var response = TaskMapper.MapToResponse(result.Data!);

            return Result<TaskResponse>.Success(response);
        }

        public async Task<Result<TaskResponse>> DeleteTask(int id)
        {
            var result = await repository.Delete(id);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);
            if (result.Data == null)
                return Result<TaskResponse>.Failure<TaskResponse>(TaskErrors.NotFound());

            var response = TaskMapper.MapToResponse(result.Data!);

            return Result<TaskResponse>.Success(response);
        }

        public async Task<Result<TaskResponse>> GetTask(int id)
        {
            var result = await repository.Get(id);

            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);
            if (result.Data == null)
                return Result<TaskResponse>.Failure<TaskResponse>(TaskErrors.NotFound());

            var response = TaskMapper.MapToResponse(result.Data!);

            return Result<TaskResponse>.Success(response);
        }

        public async Task<Result<IEnumerable<TaskResponse>>> ListTask(string? status, DateTime? dueDate)
        {
            var result = await repository.List(status, dueDate);

            if (!result.IsSuccess)
                return Result<IEnumerable<TaskResponse>>.Failure<IEnumerable<TaskResponse>>(result.Error);
            if (!result.Data!.Any())
                return Result<IEnumerable<TaskResponse>>.Failure<IEnumerable<TaskResponse>>(TaskErrors.NotFound());


            var responseList = result.Data!.Select(task => TaskMapper.MapToResponse(task));
            return Result<IEnumerable<TaskResponse>>.Success(responseList);
        }

        public async Task<Result<TaskResponse>> UpdateTask(UpdateTaskRequest task)
        {
            var taskEntity = TaskMapper.MapToEntity(task);

            var getTask = await GetTask(task.Id);
            if (!getTask.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(getTask.Error);
            if (getTask.Data == null)
                return Result<TaskResponse>.Failure<TaskResponse>(TaskErrors.NotFound());

            var validationResult = validator.Validate(taskEntity);
            if (!validationResult.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(validationResult.Error);

            var result = await repository.Update(taskEntity);
            if (!result.IsSuccess)
                return Result<TaskResponse>.Failure<TaskResponse>(result.Error);

            var response = TaskMapper.MapToResponse(result.Data!);

            return Result<TaskResponse>.Success<TaskResponse>(response);
        }
    }
}
