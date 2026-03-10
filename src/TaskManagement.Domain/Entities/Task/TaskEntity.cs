using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enum;

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
