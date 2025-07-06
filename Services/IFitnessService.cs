using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public interface IFitnessService
    {
        Task<List<Workout>> GetWorkoutsAsync();
        Task<Workout?> GetWorkoutByIdAsync(int workoutId);
        Task<List<Exercise>> GetExercisesAsync();
        Task<Exercise?> GetExerciseByIdAsync(int exerciseId);
        Task<bool> LogWorkoutAsync(WorkoutLog workoutLog);
        Task<List<WorkoutLog>> GetUserWorkoutLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<WorkoutLog?> GetWorkoutLogByIdAsync(int logId);
        Task<bool> UpdateWorkoutLogAsync(WorkoutLog workoutLog);
        Task<bool> DeleteWorkoutLogAsync(int logId);
        Task<int> GetUserTotalWorkoutsAsync(int userId);
        Task<int> GetUserTotalCaloriesBurnedAsync(int userId);
        Task<Dictionary<string, int>> GetUserWorkoutStatsAsync(int userId, int days = 30);
    }
} 