namespace TaskManagement.Api.Requests.Task
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string? Status { get; set; }
    }
}
