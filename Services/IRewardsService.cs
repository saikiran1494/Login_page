using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public interface IRewardsService
    {
        Task<List<Reward>> GetRewardsAsync();
        Task<Reward?> GetRewardByIdAsync(int rewardId);
        Task<List<UserReward>> GetUserRewardsAsync(int userId);
        Task<bool> AwardRewardAsync(int userId, int rewardId, int pointsEarned);
        Task<bool> LogDailyStreakAsync(DailyStreak dailyStreak);
        Task<DailyStreak?> GetUserDailyStreakAsync(int userId, DateTime date);
        Task<List<DailyStreak>> GetUserStreaksAsync(int userId, int days = 30);
        Task<int> GetUserTotalPointsAsync(int userId);
        Task<int> GetUserCurrentStreakAsync(int userId);
        Task<int> GetUserLongestStreakAsync(int userId);
        Task<Dictionary<string, int>> GetUserRewardStatsAsync(int userId);
        Task<bool> CheckAndAwardDailyRewardAsync(int userId);
        Task<bool> CheckAndAwardWeeklyRewardAsync(int userId);
    }
} 