using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> ValidateLoginAsync(string username, string password);
        Task<bool> SendOTPAsync(string mobileNumber, string purpose);
        Task<bool> ValidateOTPAsync(string mobileNumber, string otpCode, string purpose);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsMobileExistsAsync(string mobileNumber);
    }
} 