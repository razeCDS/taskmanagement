using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Mappings
{
    public static class TaskMapper
    {
        public static TaskEntity MapToEntity(TaskRequest request)
        {
            return new TaskEntity
            {
                Title = request.Titulo!,
                Description = request.Descricao,
                DueDate = request.DataVencimento,
                Status = request.Status!
            };
        }

        public static TaskResponse MapToResponse(TaskEntity entity)
        {
            return new TaskResponse
            {
                Id = entity.Id,
                Titulo = entity.Title,
                Descricao = entity.Description,
                DataVencimento = entity.DueDate,
                Status = entity.Status
            };
        }
    }
}
