using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class UsersListingViewModel : PageViewModel
    {
        public string SearchTerm { get; set; }
        public string RoleID { get; set; }
        public List<IdentityRole> Roles { get; set; }

        public List<eCommerceUser> Users { get; set; }
        public Pager Pager { get; set; }
    }

    public class UserDetailsViewModel : PageViewModel
    {
        public eCommerceUser User { get; set; }

        public string ID { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}