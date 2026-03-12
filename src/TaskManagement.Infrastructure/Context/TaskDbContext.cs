using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infrastructure.Context
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<TaskEntity> Task { get; set; }
    }
}
