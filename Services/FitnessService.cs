using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class FitnessService : IFitnessService
    {
        private readonly FitnessDbContext _context;

        public FitnessService(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetWorkoutsAsync()
        {
            return await _context.Workouts
                .Where(w => w.IsActive)
                .Include(w => w.Exercises)
                .ToListAsync();
        }

        public async Task<Workout?> GetWorkoutByIdAsync(int workoutId)
        {
            return await _context.Workouts
                .Where(w => w.WorkoutId == workoutId && w.IsActive)
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Exercise>> GetExercisesAsync()
        {
            return await _context.Exercises
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        public async Task<Exercise?> GetExerciseByIdAsync(int exerciseId)
        {
            return await _context.Exercises
                .Where(e => e.ExerciseId == exerciseId && e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> LogWorkoutAsync(WorkoutLog workoutLog)
        {
            try
            {
                workoutLog.CreatedTime = DateTime.Now;
                _context.WorkoutLogs.Add(workoutLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<WorkoutLog>> GetUserWorkoutLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<WorkoutLog> query = _context.WorkoutLogs
                .Where(w => w.UserId == userId)
                .Include(w => w.Workout)
                .OrderByDescending(w => w.Date);

            if (startDate.HasValue)
                query = query.Where(w => w.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(w => w.Date <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<WorkoutLog?> GetWorkoutLogByIdAsync(int logId)
        {
            return await _context.WorkoutLogs
                .Where(w => w.LogId == logId)
                .Include(w => w.Workout)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateWorkoutLogAsync(WorkoutLog workoutLog)
        {
            try
            {
                _context.WorkoutLogs.Update(workoutLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteWorkoutLogAsync(int logId)
        {
            try
            {
                var workoutLog = await _context.WorkoutLogs.FindAsync(logId);
                if (workoutLog != null)
                {
                    _context.WorkoutLogs.Remove(workoutLog);
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

        public async Task<int> GetUserTotalWorkoutsAsync(int userId)
        {
            return await _context.WorkoutLogs
                .Where(w => w.UserId == userId && w.Status == "Completed")
                .CountAsync();
        }

        public async Task<int> GetUserTotalCaloriesBurnedAsync(int userId)
        {
            return await _context.WorkoutLogs
                .Where(w => w.UserId == userId && w.Status == "Completed")
                .SumAsync(w => w.CaloriesBurned);
        }

        public async Task<Dictionary<string, int>> GetUserWorkoutStatsAsync(int userId, int days = 30)
        {
            var startDate = DateTime.Now.AddDays(-days);
            var logs = await _context.WorkoutLogs
                .Where(w => w.UserId == userId && w.Date >= startDate)
                .ToListAsync();

            var stats = new Dictionary<string, int>
            {
                ["TotalWorkouts"] = logs.Count,
                ["TotalCalories"] = logs.Sum(w => w.CaloriesBurned),
                ["TotalDuration"] = logs.Sum(w => w.Duration),
                ["AverageCalories"] = logs.Any() ? (int)Math.Round(logs.Average(w => w.CaloriesBurned), 0) : 0,
                ["AverageDuration"] = logs.Any() ? (int)Math.Round(logs.Average(w => w.Duration), 0) : 0
            };

            return stats;
        }
    }
} 