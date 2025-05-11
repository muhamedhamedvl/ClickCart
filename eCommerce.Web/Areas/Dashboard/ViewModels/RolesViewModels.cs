using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class RolesListingViewModel : PageViewModel
    {
        public string SearchTerm { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public Pager Pager { get; set; }
    }

    public class RoleDetailsViewModel : PageViewModel
    {
        public IdentityRole Role { get; set; }

        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class RoleUsersViewModel : PageViewModel
    {
        public List<eCommerceUser> RoleUsers { get; set; }

        public Pager Pager { get; set; }
        public string RoleID { get; internal set; }
    }

    public class UserRolesViewModel : PageViewModel
    {
        public List<IdentityRole> AvailableRoles { get; set; }
        public List<IdentityRole> UserRoles { get; set; }
        public eCommerceUser User { get; internal set; }
    }
}