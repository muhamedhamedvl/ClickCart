using eCommerce.Web.Areas.API.Commons.ActionResults;
using eCommerce.Web.Areas.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace eCommerce.Web.Areas.API.Commons.Extensions
{
    public static class APIExtensions
    {
        public static APIResult APIResult(this APIBaseController controller, HttpStatusCode httpStatusCode, Object data)
        {
            return new APIResult(controller.Request, httpStatusCode, data);
        }
    }
}