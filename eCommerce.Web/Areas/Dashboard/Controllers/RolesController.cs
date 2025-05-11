using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class RolesController : DashboardBaseController
    {
        private eCommerceUserManager _userManager;
        private eCommerceRoleManager _roleManager;
        private eCommerceSignInManager _signInManager;

        public RolesController()
        {
        }

        public RolesController(eCommerceUserManager userManager, eCommerceRoleManager roleManager, eCommerceSignInManager signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public eCommerceSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<eCommerceSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public eCommerceUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<eCommerceUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public eCommerceRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<eCommerceRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index(string searchTerm, int? pageNo)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            RolesListingViewModel model = new RolesListingViewModel
            {
                PageTitle = "Roles",
                PageDescription = "Roles Listing Page",

                SearchTerm = searchTerm
            };

            var roles = RoleManager.Roles;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            pageNo = pageNo ?? 1;

            var skipCount = (pageNo.Value - 1) * pageSize;

            model.Roles = roles.OrderBy(x => x.Name).Skip(skipCount).Take(pageSize).ToList();

            model.Pager = new Pager(RoleManager.Roles.Count(), pageNo, pageSize);

            return View(model);
        }

        public async Task<ActionResult> RoleDetails(string roleID)
        {
            RoleDetailsViewModel model = new RoleDetailsViewModel();

            var role = await RoleManager.FindByIdAsync(roleID);

            if (role != null)
            {
                model.Role = role;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RoleDetails", model);
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> RoleUsers(string roleID, int? pageNo)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            RoleUsersViewModel model = new RoleUsersViewModel();

            var role = await RoleManager.FindByIdAsync(roleID);

            if (role != null)
            {
                model.RoleID = role.Id;
                pageNo = pageNo ?? 1;

                var skipCount = (pageNo.Value - 1) * pageSize;
                var users = role.Users.OrderBy(x => x.UserId).Skip(skipCount).Take(pageSize);

                model.RoleUsers = new List<eCommerceUser>();
                foreach (var user in users)
                {
                    model.RoleUsers.Add(await UserManager.FindByIdAsync(user.UserId));
                }

                model.Pager = new Pager(role.Users.Count(), pageNo, pageSize);
            }

            return PartialView("_RoleUsers", model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(string roleID, string roleName)
        {
            JsonResult result = new JsonResult();

            if (!string.IsNullOrEmpty(roleName))
            {
                IdentityResult res;
                if (!string.IsNullOrEmpty(roleID))
                {
                    var role = await RoleManager.FindByIdAsync(roleID);

                    if (role != null && !role.Name.ToLower().Equals("administrator"))
                    {
                        role.Name = roleName;

                        res = await RoleManager.UpdateAsync(role);
                    }
                    else
                    {
                        result.Data = new { Success = false, Message = "Administrator role can't be modified." };
                        return result;
                    }
                }
                else
                {
                    res = await RoleManager.CreateAsync(new IdentityRole() { Name = roleName });
                }

                result.Data = new { Success = res.Succeeded, Message = string.Join(", ", res.Errors) };
                return result;
            }
            else
            {
                result.Data = new { Success = false, Message = "Please add a valid role name." };
            }

            return result;
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string roleID)
        {
            JsonResult result = new JsonResult();

            if (!string.IsNullOrEmpty(roleID))
            {
                var role = await RoleManager.FindByIdAsync(roleID);

                if (role != null && !role.Name.ToLower().Equals("administrator"))
                {
                    var res = await RoleManager.DeleteAsync(role);

                    result.Data = new { Success = res.Succeeded, Message = string.Join(", ", res.Errors) };
                    return result;
                }
                else
                {
                    result.Data = new { Success = false, Message = "Administrator role can't be modified." };
                    return result;
                }
            }

            result.Data = new { Success = false, Message = "An error has occured while deleting Role Details." };
            return result;
        }

        public async Task<ActionResult> UserRoles(string userID)
        {
            UserRolesViewModel model = new UserRolesViewModel
            {
                AvailableRoles = RoleManager.Roles.ToList()
            };

            if (!string.IsNullOrEmpty(userID))
            {
                model.User = await UserManager.FindByIdAsync(userID);

                if (model.User != null)
                {
                    model.UserRoles = model.User.Roles.Select(userRole => model.AvailableRoles.FirstOrDefault(role => role.Id == userRole.RoleId)).ToList();
                }
            }

            return PartialView("_UserRoles", model);
        }

        public async Task<ActionResult> AssignUserRole(string userID, string roleID)
        {
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID))
            {
                var user = await UserManager.FindByIdAsync(userID);

                if (user != null)
                {
                    var role = await RoleManager.FindByIdAsync(roleID);

                    if (role != null)
                    {
                        await UserManager.AddToRoleAsync(userID, role.Name);
                    }
                }
            }

            return RedirectToAction("UserRoles", new { userID = userID });
        }

        public async Task<ActionResult> RemoveUserRole(string userID, string roleID)
        {
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID))
            {
                var user = await UserManager.FindByIdAsync(userID);

                if (user != null)
                {
                    var role = await RoleManager.FindByIdAsync(roleID);

                    if (role != null)
                    {
                        await UserManager.RemoveFromRoleAsync(userID, role.Name);
                    }
                }
            }

            return RedirectToAction("UserRoles", new { userID = userID });
        }
    }
}