namespace FitnessTracker.Models
{
    public class UserReward
    {
        public int UserRewardId { get; set; }
        public int UserId { get; set; }
        public int RewardId { get; set; }
        public DateTime EarnedDate { get; set; } = DateTime.Now;
        public int PointsEarned { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Reward Reward { get; set; } = null!;
    }
} 