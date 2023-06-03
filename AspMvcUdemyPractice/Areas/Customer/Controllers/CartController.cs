using Microsoft.AspNetCore.Mvc;

namespace AspMvcUdemyPractice.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
