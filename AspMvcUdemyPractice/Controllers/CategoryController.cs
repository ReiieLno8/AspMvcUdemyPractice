﻿using AspMvcUdemyPractice.Data.Data;
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id); // Find() only works on primary key
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
                _db.Categories.Update(obj);//checking if category is valid and populated
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id); // Find() only works on primary key
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
            Category? obj = _db.Categories.Find(id);
            if (obj == null) 
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Successfully Deleted"; //for notification purposes check _Notification.cshtml
            return RedirectToAction("Index");
        }
    }
}
