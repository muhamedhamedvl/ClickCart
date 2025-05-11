using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace eCommerce.Web.Areas.API.Commons.ActionResults
{
    public class ActionExecuter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            AppDataHelper.Populate();

            base.OnActionExecuting(actionContext);
        }
    }
}