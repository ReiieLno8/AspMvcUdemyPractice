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
                productVM.Product = _unitOfWork.ProductCategory.Get(u => u.Id == id,includeProperties: "ProductImages");
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult ProductUpsert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            { 
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductCategory.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.ProductCategory.Update(productVM.Product);
                }

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath; // webrootpath will give as a path of wwwroot folder
                if (files != null)
                {
                    // Udemy section 16.201 for Image multiple upload 
                    foreach (IFormFile file in files)
                    {
                        // udemy section 6.94
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // will give as a random name to our file (this is file name)
                        string productPath = @"images\product\product-" + productVM.Product.Id; // it will give a path inside this product folder where we have to actually uploaded this "file"  (location where we save the file )
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };
                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                    }

                    _unitOfWork.ProductCategory.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
                //Note === we add "objFromDb.ProductImages = obj.ProductImages;" to ProductCategoryRepository
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


        // section 16 chapter 204
        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted Successfully";
            }

            return RedirectToAction(nameof(ProductUpsert), new { id = productId});
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

            string productPath = @"images\product\product-" + id; // it will give a path inside this product folder where we have to actually uploaded this "file"  (location where we save the file )
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                //But before we delete the directory we have to retrieve all the files in that directory and remove each file
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath); //So we are deleting the individual files here in that folder.
                }
                Directory.Delete(finalPath);//And finally we are deleting that directory.
            }

            _unitOfWork.ProductCategory.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
