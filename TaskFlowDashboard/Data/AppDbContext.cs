using Microsoft.EntityFrameworkCore;
using TaskFlowDashboard.Models;

namespace TaskFlowDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask> Tasks { get; set; }
    }
}