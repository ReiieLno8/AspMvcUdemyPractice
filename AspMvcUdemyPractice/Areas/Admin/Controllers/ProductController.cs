using AspMvcUdemyPractice.DataAccess.Repository;
using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using AspMvcUdemyPractice.Models.ViewModels;
using AspMvcUdemyPractice.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace AspMvcUdemyPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)] // even though you access the category using link you cant still access it because of authorization 
    public class ProductController: Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; //To access wwwroot
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductCategoryList = _unitOfWork.ProductCategory.GetAll(includeProperties:"Category").ToList();

            return View(objProductCategoryList);
        }

        public IActionResult ProductUpsert(int? id) // by default http is automatically Get so no need to include [https]
        {
            //converting category to innumerable of select list item using projection
            // for more info udemy Section 6.87
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.ProductCategory.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult ProductUpsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid) //checking if Product is valid and populated
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // webrootpath will give as a path of wwwroot folder
                if (file != null)
                {
                    // udemy section 6.94
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // will give as a random name to our file (this is file name)
                    var productPath = Path.Combine(wwwRootPath, @"Images\product"); // it will give a path inside this product folder where we have to actually uploaded this "file"  (location where we save the file )
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //old image path
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath)) //checking if path is exist
                        { 
                            System.IO.File.Delete(oldImagePath);// if exist deleting the image
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductCategory.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.ProductCategory.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Successfully Created.";//for notification purposes check _Notification.cshtml
                return RedirectToAction("Index"); // once the category will added we have to redirect to category Index to see all categories
            }
            else
            {
                //to populate the dropdown once you encounter an error
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }
         

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductCategoryList = _unitOfWork.ProductCategory.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductCategoryList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.ProductCategory.Get(u => u.Id == id);
            if (id == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath)) //checking if path is exist
            {
                System.IO.File.Delete(oldImagePath);// if exist deleting the image
            }

            _unitOfWork.ProductCategory.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
