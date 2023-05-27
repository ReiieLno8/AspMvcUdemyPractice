using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspMvcUdemyPractice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Product()
        {
            List<Product> objProductCategoryList = _unitOfWork.ProductCategory.GetAll().ToList();
            return View(objProductCategoryList);
        }

        public IActionResult ProductCreate() // by default http is automatically Get so no need to include [https]
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(Product obj)
        {
            if (ModelState.IsValid) //checking if Product is valid and populated
            {
                _unitOfWork.ProductCategory.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully Created.";//for notification purposes check _Notification.cshtml
                return RedirectToAction("Index"); // once the category will added we have to redirect to category Index to see all categories
            }
            return View();
        }

        public IActionResult ProductEdit(int? id) // by default http is automatically Get so no need to include [httpsGet]
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? categoryFromDb = _unitOfWork.ProductCategory.Get(u => u.Id == id); // Find() only works on primary key
            //Category? category1 = _db.Categories.FirstOrDefault(i => i.Id == id); different kinds of filtering 
            //Category? category2 = _db.Categories.Where(i => i.Id == id).FirstOrDefault(); different kinds of filtering 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult ProductEdit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductCategory.Update(obj);//checking if category is valid and populated
                _unitOfWork.Save();
                TempData["success"] = "Successfully Updated";//for notification purposes check _Notification.cshtml
                return RedirectToAction("Index");// once the category will added we have to redirect to category Index to see all categories
            }
            return View();
        }

        public IActionResult ProductDelete(int? id) // by default http is automatically Get so no need to include [httpsGet]
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProductCategoryFromDb = _unitOfWork.ProductCategory.Get(u => u.Id == id); // Find() only works on primary key
            //Category? category1 = _db.Categories.FirstOrDefault(i => i.Id == id); different kinds of filtering 
            //Category? category2 = _db.Categories.Where(i => i.Id == id).FirstOrDefault(); different kinds of filtering 
            if (ProductCategoryFromDb == null)
            {
                return NotFound();
            }
            return View(ProductCategoryFromDb);
        }
        [HttpPost, ActionName("Delete")] //explicitly name into Delete because in the form we will posting it will look for the same delete action method
        public IActionResult ProductDeletePost(int? id)// need to change name into DeletePost reason is Delete have the same parameter and name
        {
            Product? obj = _unitOfWork.ProductCategory.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductCategory.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Successfully Deleted"; //for notification purposes check _Notification.cshtml
            return RedirectToAction("Product");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}