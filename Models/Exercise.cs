namespace FitnessTracker.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty; // "Strength", "Cardio", "Flexibility"
        public string MuscleGroup { get; set; } = string.Empty; // "Chest", "Back", "Legs", "Arms", "Core"
        public string Equipment { get; set; } = string.Empty; // "Dumbbells", "Barbell", "Bodyweight", "Machine"
        public string Instructions { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
} 