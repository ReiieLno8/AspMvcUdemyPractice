using System.ComponentModel.DataAnnotations;

namespace AspMvcUdemyPractice.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        public int CategoryID { get; set; }
    }
}
