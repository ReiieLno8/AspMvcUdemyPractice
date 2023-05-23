using AspMvcUdemyPractice.Data;
using AspMvcUdemyPractice.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspMvcUdemyPractice.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString()) // Custom validation 
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid) //checking if category is valid and populated
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index"); // once the category will added we have to redirect to category Index to see all categories
            }
            return View();
        }
    }
}
