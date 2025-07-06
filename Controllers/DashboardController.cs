using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Services;
using Microsoft.AspNetCore.Http;

namespace FitnessTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IFitnessService _fitnessService;
        private readonly IDietService _dietService;
        private readonly IRewardsService _rewardsService;

        public DashboardController(IFitnessService fitnessService, IDietService dietService, IRewardsService rewardsService)
        {
            _fitnessService = fitnessService;
            _dietService = dietService;
            _rewardsService = rewardsService;
        }

        // GET: Dashboard/Index
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdInt = int.Parse(userId);

            // Get dashboard statistics
            var workoutStats = await _fitnessService.GetUserWorkoutStatsAsync(userIdInt, 30);
            var nutritionStats = await _dietService.GetUserNutritionStatsAsync(userIdInt, 30);
            var rewardStats = await _rewardsService.GetUserRewardStatsAsync(userIdInt);

            // Get recent activities
            var recentWorkouts = await _fitnessService.GetUserWorkoutLogsAsync(userIdInt, null, null);
            var recentMeals = await _dietService.GetUserMealLogsAsync(userIdInt, null, null);

            ViewBag.WorkoutStats = workoutStats;
            ViewBag.NutritionStats = nutritionStats;
            ViewBag.RewardStats = rewardStats;
            ViewBag.RecentWorkouts = recentWorkouts.Take(5).ToList();
            ViewBag.RecentMeals = recentMeals.Take(5).ToList();

            return View();
        }

        // GET: Dashboard/Profile
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.FullName = HttpContext.Session.GetString("FullName");

            return View();
        }

        // GET: Dashboard/Settings
        public IActionResult Settings()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // GET: Dashboard/Statistics
        public async Task<IActionResult> Statistics()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdInt = int.Parse(userId);

            // Get comprehensive statistics
            var workoutStats = await _fitnessService.GetUserWorkoutStatsAsync(userIdInt, 90);
            var nutritionStats = await _dietService.GetUserNutritionStatsAsync(userIdInt, 90);
            var rewardStats = await _rewardsService.GetUserRewardStatsAsync(userIdInt);

            // Get weekly and monthly data for charts
            var weeklyWorkouts = await _fitnessService.GetUserWorkoutLogsAsync(userIdInt, DateTime.Now.AddDays(-7), DateTime.Now);
            var weeklyMeals = await _dietService.GetUserMealLogsAsync(userIdInt, DateTime.Now.AddDays(-7), DateTime.Now);

            ViewBag.WorkoutStats = workoutStats;
            ViewBag.NutritionStats = nutritionStats;
            ViewBag.RewardStats = rewardStats;
            ViewBag.WeeklyWorkouts = weeklyWorkouts;
            ViewBag.WeeklyMeals = weeklyMeals;

            return View();
        }

        // GET: Dashboard/Goals
        public IActionResult Goals()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Dashboard/UpdateGoals
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGoals(string goalType, int targetValue)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Here you would implement goal setting logic
            TempData["SuccessMessage"] = "Goals updated successfully!";
            return RedirectToAction("Goals");
        }

        // GET: Dashboard/Notifications
        public IActionResult Notifications()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // GET: Dashboard/Help
        public IActionResult Help()
        {
            return View();
        }
    }
} 