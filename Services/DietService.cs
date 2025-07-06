using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class DietService : IDietService
    {
        private readonly FitnessDbContext _context;

        public DietService(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<List<Diet>> GetDietsAsync()
        {
            return await _context.Diets
                .Where(d => d.IsActive)
                .Include(d => d.Meals)
                .ToListAsync();
        }

        public async Task<Diet?> GetDietByIdAsync(int dietId)
        {
            return await _context.Diets
                .Where(d => d.DietId == dietId && d.IsActive)
                .Include(d => d.Meals)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Meal>> GetMealsAsync()
        {
            return await _context.Meals
                .Where(m => m.IsActive)
                .Include(m => m.Diet)
                .ToListAsync();
        }

        public async Task<Meal?> GetMealByIdAsync(int mealId)
        {
            return await _context.Meals
                .Where(m => m.MealId == mealId && m.IsActive)
                .Include(m => m.Diet)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Meal>> GetMealsByDietAsync(int dietId)
        {
            return await _context.Meals
                .Where(m => m.DietId == dietId && m.IsActive)
                .ToListAsync();
        }

        public async Task<bool> LogMealAsync(MealLog mealLog)
        {
            try
            {
                mealLog.CreatedTime = DateTime.Now;
                _context.MealLogs.Add(mealLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<MealLog>> GetUserMealLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<MealLog> query = _context.MealLogs
                .Where(m => m.UserId == userId)
                .Include(m => m.Meal)
                .OrderByDescending(m => m.Date);

            if (startDate.HasValue)
                query = query.Where(m => m.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.Date <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<MealLog?> GetMealLogByIdAsync(int logId)
        {
            return await _context.MealLogs
                .Where(m => m.LogId == logId)
                .Include(m => m.Meal)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMealLogAsync(MealLog mealLog)
        {
            try
            {
                _context.MealLogs.Update(mealLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMealLogAsync(int logId)
        {
            try
            {
                var mealLog = await _context.MealLogs.FindAsync(logId);
                if (mealLog != null)
                {
                    _context.MealLogs.Remove(mealLog);
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

        public async Task<Dictionary<string, decimal>> GetUserNutritionStatsAsync(int userId, int days = 30)
        {
            var startDate = DateTime.Now.AddDays(-days);
            var logs = await _context.MealLogs
                .Where(m => m.UserId == userId && m.Date >= startDate)
                .ToListAsync();

            var stats = new Dictionary<string, decimal>
            {
                ["TotalCalories"] = logs.Sum(m => (decimal)m.Calories),
                ["TotalProtein"] = logs.Sum(m => (decimal)m.Protein),
                ["TotalCarbohydrates"] = logs.Sum(m => (decimal)m.Carbohydrates),
                ["TotalFat"] = logs.Sum(m => (decimal)m.Fat),
                ["TotalFiber"] = logs.Sum(m => (decimal)m.Fiber),
                ["AverageCalories"] = logs.Any() ? (decimal)logs.Average(m => m.Calories) : 0,
                ["AverageProtein"] = logs.Any() ? (decimal)logs.Average(m => m.Protein) : 0,
                ["AverageCarbohydrates"] = logs.Any() ? (decimal)logs.Average(m => m.Carbohydrates) : 0,
                ["AverageFat"] = logs.Any() ? (decimal)logs.Average(m => m.Fat) : 0,
                ["AverageFiber"] = logs.Any() ? (decimal)logs.Average(m => m.Fiber) : 0
            };

            return stats;
        }

        public async Task<int> GetUserTotalCaloriesConsumedAsync(int userId)
        {
            return await _context.MealLogs
                .Where(m => m.UserId == userId)
                .SumAsync(m => m.Calories);
        }

        public async Task<Dictionary<string, int>> GetUserMealStatsAsync(int userId, int days = 30)
        {
            var startDate = DateTime.Now.AddDays(-days);
            var logs = await _context.MealLogs
                .Where(m => m.UserId == userId && m.Date >= startDate)
                .ToListAsync();

            var stats = new Dictionary<string, int>
            {
                ["TotalMeals"] = logs.Count,
                ["BreakfastCount"] = logs.Count(m => m.MealType == "Breakfast"),
                ["LunchCount"] = logs.Count(m => m.MealType == "Lunch"),
                ["DinnerCount"] = logs.Count(m => m.MealType == "Dinner"),
                ["SnackCount"] = logs.Count(m => m.MealType == "Snack"),
                ["AverageCaloriesPerMeal"] = logs.Any() ? (int)Math.Round(logs.Average(m => m.Calories), 0) : 0
            };

            return stats;
        }
    }
} 