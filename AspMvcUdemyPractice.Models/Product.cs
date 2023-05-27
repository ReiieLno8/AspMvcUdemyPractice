using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")] // so show ListPrice with spaces
        [Range(1,1000)]
        public int ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Price { get; set; }

        [Required]
        [Display(Name = "For Price 50+")] // to include spaces
        [Range(1, 1000)]
        public int Price50 { get; set; }

        [Required]
        [Display(Name = "For Price Price")] // to include spaces
        [Range(1, 1000)]
        public int Price100 { get; set; }
    }
}
