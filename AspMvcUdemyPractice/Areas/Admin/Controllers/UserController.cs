using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.DataAccess.Repository.IRepository;
using AspMvcUdemyPractice.Models;
using AspMvcUdemyPractice.Models.ViewModels;
using AspMvcUdemyPractice.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using AspMvcUdemyPractice.DataAccess.Repository;
using Stripe;
using System.Buffers.Text;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;
using Stripe.Radar;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AspMvcUdemyPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        //private readonly ApplicationDbContext _db; //normally we are using UnitofWork and this is another kind of route instructor dont recommend this route
        //private readonly UserManager<IdentityUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {

            //When we are in there, we have to populate the role management view model.
            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties:"Company"),
                //Then next we have to populate the dropdowns for row list and company list.
                roleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.CompanyCategory.GetAll().Select(i => new SelectListItem 
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            //First, we will use user manager.
            //On there we have the method get roles async that will get roles assigned to a user.
            //Now, in our case, we only assign one role to the user so we can do first our default and get the firstone.
            //But inside here you can see it expects the user object.
            //So we basically need to retrieve the user unit of work that application user dot get.
            //We will get that based on the user ID, so you.id is equal equal to.
            //And where is that user ID?
            //We have that in the parameter here so we can pass that.
            //And this is an async method, so we will have to call the get awaiter dot get result..
            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u=>u.Id == userId)).GetAwaiter().GetResult().FirstOrDefault();

            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            //retrieve old role from the user
            string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleManagementVM.ApplicationUser.Id)).GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagementVM.ApplicationUser.Id);

            if (!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                // a Role was updated
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                // if the old rule was a company rule, then we have to remove that company ID
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                //And now we are not working with underscore DB because of that we will have to explicitly call unit of
                //work dot application user dot update.
                //And we will pass the application user that we retrieved and modified right here.
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();

                //finally, we have to remove the old role and assign the new role to our user.
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                //if application user company ID is not equal to the company ID that is selected in the dropdown that is populated from
                //the input field, that means they have updated the company.
                if (oldRole==SD.Role_Company && applicationUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
                {
                    //So in that case we need to assign new company ID, update the application user and save the changes.
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }

            return RedirectToAction("Index");
        }

        #region Api
        [HttpGet]
        public IActionResult GetAll()
        {
            //section 15 190-191
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

            foreach (var user in objUserList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { Success = true, message = "Lock Successful" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1);
            }
            //So update first and then save the changes.
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { Success = true, message = "Unlock Successful" });
        }

        #endregion
    }
}