using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enum;

namespace TaskManagement.Api.Requests.Task
{
    public class CreateTaskRequest
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string? Status { get; set; }
    }
}
