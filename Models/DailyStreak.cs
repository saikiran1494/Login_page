namespace FitnessTracker.Models
{
    public class DailyStreak
    {
        public int StreakId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool WorkoutCompleted { get; set; } = false;
        public bool DietFollowed { get; set; } = false;
        public int PointsEarned { get; set; } = 0;
        public string? Notes { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
} 