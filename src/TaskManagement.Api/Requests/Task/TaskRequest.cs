namespace TaskManagement.Api.Requests.Task
{
    public class CreateTaskRequest
    {
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public required string Status { get; set; }
    }
}
