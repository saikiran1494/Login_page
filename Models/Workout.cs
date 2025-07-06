namespace FitnessTracker.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty; // "Strength", "Cardio", "Flexibility", "Mixed"
        public int Duration { get; set; } // in minutes
        public string Difficulty { get; set; } = string.Empty; // "Beginner", "Intermediate", "Advanced"
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
    }
} 