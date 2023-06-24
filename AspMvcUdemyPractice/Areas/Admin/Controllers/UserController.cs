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

namespace AspMvcUdemyPractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db; //normally we are using UnitofWork and this is another kind of route instructor dont recommend this route
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            //When we are in there, we have to populate the rule management view model.
            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                //Then next we have to populate the dropdowns for row list and company list.
                roleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companies.Select(i => new SelectListItem 
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            // After all of that inside the role view model, we have application user and there we have a role of the user that we will populate using underscore DB dot roles based on the role ID that we retrieved here and we want to extract name from there.
            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            if (!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                // a Role was updated
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.ApplicationUser.Id);
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                // if the old rule was a company rule, then we have to remove that company ID
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();

                //finally, we have to remove the old role and assign the new role to our user.
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }

        #region Api
        [HttpGet]
        public IActionResult GetAll()
        {
            //section 15 190-191
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var UserRoles = _db.UserRoles.ToList(); // Accessing AspNetUserRoles from database
            var roles = _db.Roles.ToList();// Accessing AspNetRoles from database

            foreach (var user in objUserList)
            {
                var roleId = UserRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId; //the .RoleId means that we will retrieve the RoleId
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name; // the .name means that we will retrieve the name

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
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { Success = true, message = "Lock Successfull" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1);
            }
            _db.SaveChanges();
            return Json(new { Success = true, message = "Unlock Successfull" });
        }

        #endregion
    }
}