using System.ComponentModel.DataAnnotations;

namespace FLMBlazorWebApp.Repositories
{
    public class Product
    {
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = "ID is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public bool WeightedItem { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Suggested Selling Price is required")]
        public decimal SuggestedSellingPrice { get; set; }
    }
}
