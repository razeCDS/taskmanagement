namespace TaskManagement.Domain.Entities.Task
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public required string Status { get; set; }
    }
}
