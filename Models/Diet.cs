namespace FitnessTracker.Models
{
    public class Diet
    {
        public int DietId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty; // "Weight Loss", "Muscle Gain", "Maintenance", "Vegetarian", "Vegan"
        public int DailyCalories { get; set; }
        public decimal ProteinPercentage { get; set; } // percentage of daily calories
        public decimal CarbohydratePercentage { get; set; }
        public decimal FatPercentage { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
} 