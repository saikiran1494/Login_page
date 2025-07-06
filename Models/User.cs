using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FullName { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginDate { get; set; }

        // Navigation properties
        public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
        public virtual ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
        public virtual ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
        public virtual ICollection<DailyStreak> DailyStreaks { get; set; } = new List<DailyStreak>();
    }
} 