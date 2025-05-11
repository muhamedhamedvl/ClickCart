using System.Web.Mvc;

namespace eCommerce.Web.Areas.API
{
    public class APIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "API";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            /* We don't need to create routes for API area because we are handling API routes in WebApiConfig class. */

            //context.MapRoute(
            //    name: "API_Default",
            //    url: "API/{controller}/{action}/",
            //    defaults: new { controller = "APIBaseController", action = "Index" }
            //);
        }
    }
}