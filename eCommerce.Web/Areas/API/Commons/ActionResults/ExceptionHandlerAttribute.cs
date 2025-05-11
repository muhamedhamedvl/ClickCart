using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace eCommerce.Web.Areas.API.Commons.ActionResults
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if(ConfigurationsHelper.Environment == Shared.Enums.Environments.LIVE)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new Result()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        StatusMessage = HttpStatusCode.InternalServerError.ToString().MakeWord(),
                    }), Encoding.UTF8, "application/json")
                };
            }
        }

        private class Result
        {
            [JsonProperty(Order = 1)]
            public int StatusCode { get; set; }

            [JsonProperty(Order = 2)]
            public string StatusMessage { get; set; }
        }
    }
}