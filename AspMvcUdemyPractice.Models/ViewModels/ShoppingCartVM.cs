using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Models.ViewModels
{
    // reason why we create this because we only have product ID and user ID and product details in the shopping cart model we also want to show the order total and other details
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartsList { get; set; }
        public double OrderTotal { get; set; }

    }
}
