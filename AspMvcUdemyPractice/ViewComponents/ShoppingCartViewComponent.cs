using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

//Section 13 172
namespace AspMvcUdemyPractice.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    // when a user now logs in, they will be able to see their shopping cart count.
                    HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCartCategory.GetAll(u => u.ApplicationUserId == claim.Value).Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart)); //returning the actual shoppingcart count
            }
            else
            { 
                HttpContext.Session.Clear(); // reset shoppingcart counter
                return View(0);
            }
        }
    }
}
