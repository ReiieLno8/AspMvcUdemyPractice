using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.DataAccess.Repository;
using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspMvcUdemyPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create() // by default http is automatically Get so no need to include [https]
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
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully Created.";//for notification purposes check _Notification.cshtml
                return RedirectToAction("Index"); // once the category will added we have to redirect to category Index to see all categories
            }
            return View();
        }
        public IActionResult Edit(int? id) // by default http is automatically Get so no need to include [httpsGet]
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); // Find() only works on primary key
            //Category? category1 = _db.Categories.FirstOrDefault(i => i.Id == id); different kinds of filtering 
            //Category? category2 = _db.Categories.Where(i => i.Id == id).FirstOrDefault(); different kinds of filtering 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);//checking if category is valid and populated
                _unitOfWork.Save();
                TempData["success"] = "Successfully Updated";//for notification purposes check _Notification.cshtml
                return RedirectToAction("Index");// once the category will added we have to redirect to category Index to see all categories
            }
            return View();
        }
        public IActionResult Delete(int? id) // by default http is automatically Get so no need to include [httpsGet]
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id); // Find() only works on primary key
            //Category? category1 = _db.Categories.FirstOrDefault(i => i.Id == id); different kinds of filtering 
            //Category? category2 = _db.Categories.Where(i => i.Id == id).FirstOrDefault(); different kinds of filtering 
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")] //explicitly name into Delete because in the form we will posting it will look for the same delete action method
        public IActionResult DeletePost(int? id)// need to change name into DeletePost reason is Delete have the same parameter and name
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Successfully Deleted"; //for notification purposes check _Notification.cshtml
            return RedirectToAction("Index");
        }
    }
}
