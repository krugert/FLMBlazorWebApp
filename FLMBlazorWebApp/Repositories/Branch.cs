using System.ComponentModel.DataAnnotations;

namespace FLMBlazorWebApp.Repositories
{
    public class Branch
    {
        [Key]
        [Range(1, int.MaxValue, ErrorMessage = "ID is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Telephone Number is required")]
        [MaxLength(15, ErrorMessage = "Please enter a valid Telephone Number")]
        public string TelephoneNumber { get; set; }
        public DateTime OpenDate { get; set; }
    }
}
