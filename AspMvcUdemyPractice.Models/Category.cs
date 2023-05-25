using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspMvcUdemyPractice.Models
{
    public class Category
    {
        [Key] // data annotations.
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public String Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage = "Invalid Input it should be greater than 1 or less than 100")]
        public int DisplayOrder { get; set; }
    }
}
