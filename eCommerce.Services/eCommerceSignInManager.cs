using eCommerce.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class eCommerceSignInManager : SignInManager<eCommerceUser, string>
    {
        public eCommerceSignInManager(eCommerceUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(eCommerceUser user)
        {
            return user.GenerateUserIdentityAsync((eCommerceUserManager)UserManager);
        }

        public static eCommerceSignInManager Create(IdentityFactoryOptions<eCommerceSignInManager> options, IOwinContext context)
        {
            return new eCommerceSignInManager(context.GetUserManager<eCommerceUserManager>(), context.Authentication);
        }
    }
}
