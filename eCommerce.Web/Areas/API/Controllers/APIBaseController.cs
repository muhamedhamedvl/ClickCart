using eCommerce.Web.Areas.API.Commons.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eCommerce.Web.Areas.API.Controllers
{
    [ExceptionHandlerAttribute]
    [ActionExecuter]
    public class APIBaseController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Default()
        {
            return APIResult(HttpStatusCode.OK);
        }

        protected APIResult APIResult(HttpStatusCode httpStatusCode, Object data = null)
        {
            return new APIResult(Request, httpStatusCode, data);
        }
    }
}
