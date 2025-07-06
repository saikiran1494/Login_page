namespace FitnessTracker.Models
{
    public class UserOTP
    {
        public int OTPId { get; set; }
        public int UserId { get; set; }
        public string OTPCode { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;
        public string Purpose { get; set; } = string.Empty; // "Login", "Registration", "PasswordReset"

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
} 