using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class RewardsService : IRewardsService
    {
        private readonly FitnessDbContext _context;
        private readonly IConfiguration _configuration;

        public RewardsService(FitnessDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<Reward>> GetRewardsAsync()
        {
            return await _context.Rewards
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<Reward?> GetRewardByIdAsync(int rewardId)
        {
            return await _context.Rewards
                .Where(r => r.RewardId == rewardId && r.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserReward>> GetUserRewardsAsync(int userId)
        {
            return await _context.UserRewards
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Reward)
                .OrderByDescending(ur => ur.EarnedDate)
                .ToListAsync();
        }

        public async Task<bool> AwardRewardAsync(int userId, int rewardId, int pointsEarned)
        {
            try
            {
                var userReward = new UserReward
                {
                    UserId = userId,
                    RewardId = rewardId,
                    PointsEarned = pointsEarned,
                    EarnedDate = DateTime.Now
                };

                _context.UserRewards.Add(userReward);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LogDailyStreakAsync(DailyStreak dailyStreak)
        {
            try
            {
                dailyStreak.CreatedTime = DateTime.Now;
                _context.DailyStreaks.Add(dailyStreak);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DailyStreak?> GetUserDailyStreakAsync(int userId, DateTime date)
        {
            return await _context.DailyStreaks
                .Where(ds => ds.UserId == userId && ds.Date.Date == date.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DailyStreak>> GetUserStreaksAsync(int userId, int days = 30)
        {
            var startDate = DateTime.Now.AddDays(-days);
            return await _context.DailyStreaks
                .Where(ds => ds.UserId == userId && ds.Date >= startDate)
                .OrderByDescending(ds => ds.Date)
                .ToListAsync();
        }

        public async Task<int> GetUserTotalPointsAsync(int userId)
        {
            return await _context.UserRewards
                .Where(ur => ur.UserId == userId)
                .SumAsync(ur => ur.PointsEarned);
        }

        public async Task<int> GetUserCurrentStreakAsync(int userId)
        {
            var streaks = await _context.DailyStreaks
                .Where(ds => ds.UserId == userId)
                .OrderByDescending(ds => ds.Date)
                .ToListAsync();

            int currentStreak = 0;
            var currentDate = DateTime.Now.Date;

            foreach (var streak in streaks)
            {
                if (streak.Date.Date == currentDate && (streak.WorkoutCompleted || streak.DietFollowed))
                {
                    currentStreak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else if (streak.Date.Date < currentDate)
                {
                    break;
                }
            }

            return currentStreak;
        }

        public async Task<int> GetUserLongestStreakAsync(int userId)
        {
            var streaks = await _context.DailyStreaks
                .Where(ds => ds.UserId == userId)
                .OrderBy(ds => ds.Date)
                .ToListAsync();

            int longestStreak = 0;
            int currentStreak = 0;

            foreach (var streak in streaks)
            {
                if (streak.WorkoutCompleted || streak.DietFollowed)
                {
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else
                {
                    currentStreak = 0;
                }
            }

            return longestStreak;
        }

        public async Task<Dictionary<string, int>> GetUserRewardStatsAsync(int userId)
        {
            var userRewards = await GetUserRewardsAsync(userId);
            var totalPoints = await GetUserTotalPointsAsync(userId);
            var currentStreak = await GetUserCurrentStreakAsync(userId);
            var longestStreak = await GetUserLongestStreakAsync(userId);

            var stats = new Dictionary<string, int>
            {
                ["TotalRewards"] = userRewards.Count,
                ["TotalPoints"] = totalPoints,
                ["CurrentStreak"] = currentStreak,
                ["LongestStreak"] = longestStreak,
                ["DailyRewards"] = userRewards.Count(ur => ur.Reward.Category == "Daily"),
                ["WeeklyRewards"] = userRewards.Count(ur => ur.Reward.Category == "Weekly"),
                ["MonthlyRewards"] = userRewards.Count(ur => ur.Reward.Category == "Monthly"),
                ["AchievementRewards"] = userRewards.Count(ur => ur.Reward.Category == "Achievement")
            };

            return stats;
        }

        public async Task<bool> CheckAndAwardDailyRewardAsync(int userId)
        {
            try
            {
                var today = DateTime.Now.Date;
                var existingStreak = await GetUserDailyStreakAsync(userId, today);

                if (existingStreak == null)
                {
                    var dailyPoints = _configuration.GetValue<int>("RewardsSettings:DailyPoints");
                    var streakBonus = _configuration.GetValue<int>("RewardsSettings:StreakBonus");
                    var currentStreak = await GetUserCurrentStreakAsync(userId);
                    var totalPoints = dailyPoints + (currentStreak * streakBonus);

                    // Find daily reward
                    var dailyReward = await _context.Rewards
                        .Where(r => r.Category == "Daily" && r.IsActive)
                        .FirstOrDefaultAsync();

                    if (dailyReward != null)
                    {
                        await AwardRewardAsync(userId, dailyReward.RewardId, totalPoints);
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckAndAwardWeeklyRewardAsync(int userId)
        {
            try
            {
                var currentStreak = await GetUserCurrentStreakAsync(userId);
                var weeklyBonus = _configuration.GetValue<int>("RewardsSettings:WeeklyBonus");

                if (currentStreak >= 7)
                {
                    // Find weekly reward
                    var weeklyReward = await _context.Rewards
                        .Where(r => r.Category == "Weekly" && r.IsActive)
                        .FirstOrDefaultAsync();

                    if (weeklyReward != null)
                    {
                        await AwardRewardAsync(userId, weeklyReward.RewardId, weeklyBonus);
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
} 