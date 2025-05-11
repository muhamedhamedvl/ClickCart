using eCommerce.Shared.Attributes;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    [PageDetail]
    [CustomAuthorize(Roles = "Administrator")]
    public class DashboardBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AppDataHelper.Populate();

            base.OnActionExecuting(filterContext);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;

        //    #if DEBUG
        //    throw ex;
        //    #endif

        //    //Log Exception e
        //    filterContext.ExceptionHandled = true;
        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error"
        //    };
        //}

        [AllowAnonymous]
        public ActionResult UnAuthorized()
        {
            Response.StatusCode = 403;

            return PartialView("UnAuthorized");
        }
    }
}