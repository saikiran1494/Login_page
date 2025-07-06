using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Models
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealLog> MealLogs { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<UserReward> UserRewards { get; set; }
        public DbSet<DailyStreak> DailyStreaks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(15);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.MobileNumber).IsUnique();
            });

            // UserOTP configuration
            modelBuilder.Entity<UserOTP>(entity =>
            {
                entity.HasKey(e => e.OTPId);
                entity.Property(e => e.OTPCode).IsRequired().HasMaxLength(6);
                entity.Property(e => e.ExpiryTime).IsRequired();
            });

            // Workout configuration
            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(e => e.WorkoutId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Exercise configuration
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.ExerciseId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).HasMaxLength(50);
            });

            // WorkoutLog configuration
            modelBuilder.Entity<WorkoutLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Date).IsRequired();
            });

            // Diet configuration
            modelBuilder.Entity<Diet>(entity =>
            {
                entity.HasKey(e => e.DietId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Meal configuration
            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => e.MealId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // MealLog configuration
            modelBuilder.Entity<MealLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Date).IsRequired();
            });

            // Reward configuration
            modelBuilder.Entity<Reward>(entity =>
            {
                entity.HasKey(e => e.RewardId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // UserReward configuration
            modelBuilder.Entity<UserReward>(entity =>
            {
                entity.HasKey(e => e.UserRewardId);
                entity.Property(e => e.EarnedDate).IsRequired();
            });

            // DailyStreak configuration
            modelBuilder.Entity<DailyStreak>(entity =>
            {
                entity.HasKey(e => e.StreakId);
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
} 