namespace FitnessTracker.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public int DietId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MealType { get; set; } = string.Empty; // "Breakfast", "Lunch", "Dinner", "Snack"
        public int Calories { get; set; }
        public decimal Protein { get; set; } // in grams
        public decimal Carbohydrates { get; set; } // in grams
        public decimal Fat { get; set; } // in grams
        public decimal Fiber { get; set; } // in grams
        public string? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Diet Diet { get; set; } = null!;
        public virtual ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
    }
} 