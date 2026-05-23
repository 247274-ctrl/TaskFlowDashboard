using System.ComponentModel.DataAnnotations;

namespace TaskFlowDashboard.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}