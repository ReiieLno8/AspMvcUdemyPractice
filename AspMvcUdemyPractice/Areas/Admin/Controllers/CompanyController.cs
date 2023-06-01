using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using AspMvcUdemyPractice.Models.ViewModels;
using AspMvcUdemyPractice.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspMvcUdemyPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Company)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyCategoryList = _unitOfWork.CompanyCategory.GetAll().ToList();
            return View(objCompanyCategoryList);
        }

        // to get the details from the DB and auto populate it in the textbox we need to get the ID using the parameter "companyID" note: make sure that the name of the parameter is the same.
        public IActionResult CompanyUpsert(int? id)
        {
            if (id == null)
            {
                return View(new Company());
            }
            else
            {
                Company company = _unitOfWork.CompanyCategory.Get(u => u.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult CompanyUpsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    _unitOfWork.CompanyCategory.Add(companyObj);
                }
                else
                {
                    _unitOfWork.CompanyCategory.Update(companyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Successfully Created.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }

        #region Api
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyCategoryList = _unitOfWork.CompanyCategory.GetAll().ToList();
            return Json(new { data = objCompanyCategoryList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.CompanyCategory.Get(u => u.Id == id);
            if (id == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.CompanyCategory.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { Success = true, message = "Delete Successfull" });
        }

        #endregion
    }
}