using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FitnessTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly FitnessDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(FitnessDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                // Hash the password
                user.Password = HashPassword(user.Password);
                user.RegistrationDate = DateTime.Now;
                user.IsActive = true;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> ValidateLoginAsync(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword && u.IsActive);

            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task<bool> SendOTPAsync(string mobileNumber, string purpose)
        {
            try
            {
                // Generate OTP
                var otpCode = GenerateOTP();
                var expiryMinutes = _configuration.GetValue<int>("OTPSettings:ExpiryMinutes");
                var expiryTime = DateTime.Now.AddMinutes(expiryMinutes);

                // Find user by mobile number
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
                if (user == null) return false;

                // Create OTP record
                var userOTP = new UserOTP
                {
                    UserId = user.UserId,
                    OTPCode = otpCode,
                    CreatedTime = DateTime.Now,
                    ExpiryTime = expiryTime,
                    IsUsed = false,
                    Purpose = purpose
                };

                _context.UserOTPs.Add(userOTP);
                await _context.SaveChangesAsync();

                // In a real application, you would send SMS here
                // For demo purposes, we'll just log it
                Console.WriteLine($"OTP for {mobileNumber}: {otpCode}");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateOTPAsync(string mobileNumber, string otpCode, string purpose)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
                if (user == null) return false;

                var otp = await _context.UserOTPs
                    .Where(o => o.UserId == user.UserId && 
                               o.OTPCode == otpCode && 
                               o.Purpose == purpose && 
                               !o.IsUsed && 
                               o.ExpiryTime > DateTime.Now)
                    .OrderByDescending(o => o.CreatedTime)
                    .FirstOrDefaultAsync();

                if (otp != null)
                {
                    otp.IsUsed = true;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsMobileExistsAsync(string mobileNumber)
        {
            return await _context.Users.AnyAsync(u => u.MobileNumber == mobileNumber);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string GenerateOTP()
        {
            var random = new Random();
            var otpLength = _configuration.GetValue<int>("OTPSettings:Length");
            return random.Next(100000, 999999).ToString();
        }
    }
} 