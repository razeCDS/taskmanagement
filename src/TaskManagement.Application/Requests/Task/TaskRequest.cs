using Swashbuckle.AspNetCore.Annotations;

namespace TaskManagement.Application.Requests.Task
{
    public class TaskRequest
    {
        [SwaggerSchema(Description = "Título da tarefa")]
        public string? Titulo { get; set; }
        [SwaggerSchema(Description = "Descrição da tarefa")]
        public string? Descricao { get; set; }
        [SwaggerSchema(Description = "Data de vencimento da tarefa")]
        public DateTime? DataVencimento { get; set; }
        [SwaggerSchema(Description = "Status", Format = "Pendente | Em progresso | Concluída")]
        public string? Status { get; set; }
    }
}
