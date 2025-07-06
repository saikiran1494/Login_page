using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public interface IDietService
    {
        Task<List<Diet>> GetDietsAsync();
        Task<Diet?> GetDietByIdAsync(int dietId);
        Task<List<Meal>> GetMealsAsync();
        Task<Meal?> GetMealByIdAsync(int mealId);
        Task<List<Meal>> GetMealsByDietAsync(int dietId);
        Task<bool> LogMealAsync(MealLog mealLog);
        Task<List<MealLog>> GetUserMealLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<MealLog?> GetMealLogByIdAsync(int logId);
        Task<bool> UpdateMealLogAsync(MealLog mealLog);
        Task<bool> DeleteMealLogAsync(int logId);
        Task<Dictionary<string, decimal>> GetUserNutritionStatsAsync(int userId, int days = 30);
        Task<int> GetUserTotalCaloriesConsumedAsync(int userId);
        Task<Dictionary<string, int>> GetUserMealStatsAsync(int userId, int days = 30);
    }
} 