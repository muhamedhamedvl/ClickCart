using eCommerce.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class eCommerceRoleManager : RoleManager<IdentityRole>
    {
        public eCommerceRoleManager(IRoleStore<IdentityRole, string> roleStore) : base(roleStore)
        {
        }

        public static eCommerceRoleManager Create(IdentityFactoryOptions<eCommerceRoleManager> options, IOwinContext context)
        {
            return new eCommerceRoleManager(new RoleStore<IdentityRole>(context.Get<eCommerceContext>()));
        }
    }
}
