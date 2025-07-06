namespace FitnessTracker.Models
{
    public class Reward
    {
        public int RewardId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty; // "Daily", "Weekly", "Monthly", "Achievement"
        public int PointsRequired { get; set; }
        public string? BadgeIcon { get; set; }
        public string? BadgeColor { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
    }
} 