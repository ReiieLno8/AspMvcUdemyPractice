using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        //we can create list of product image where we can carry or load all of the images for our product
        //So that way we are telling entity framework core about the one to many relation that we have between
        //product and product image.
        public List<ProductImage> ProductImages { get; set; }
    }
}
