using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities.Task
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public required string Status { get; set; }
    }
}
