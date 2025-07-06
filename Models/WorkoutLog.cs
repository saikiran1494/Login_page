namespace FitnessTracker.Models
{
    public class WorkoutLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public int WorkoutId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Duration { get; set; } // in minutes
        public int CaloriesBurned { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Completed"; // "Completed", "In Progress", "Skipped"
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Workout Workout { get; set; } = null!;
    }
} 