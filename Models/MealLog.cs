namespace FitnessTracker.Models
{
    public class MealLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public int MealId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string MealType { get; set; } = string.Empty; // "Breakfast", "Lunch", "Dinner", "Snack"
        public int Calories { get; set; }
        public decimal Protein { get; set; } // in grams
        public decimal Carbohydrates { get; set; } // in grams
        public decimal Fat { get; set; } // in grams
        public decimal Fiber { get; set; } // in grams
        public string? Notes { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Meal Meal { get; set; } = null!;
    }
} 