using eCommerce.Shared.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace eCommerce.Web.Areas.API.Commons.ActionResults
{
    public class APIResult : IHttpActionResult
    {
        public HttpStatusCode HttpStatusCode { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public object Data { get; private set; }

        public APIResult(HttpRequestMessage request, HttpStatusCode httpStatusCode, Object data = null)
        {
            this.Request = request;
            this.HttpStatusCode = httpStatusCode;
            this.Data = data;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode);

            if(Data != null)
            {
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new ResultWithData()
                        {
                            StatusCode = (int)HttpStatusCode,
                            StatusMessage = HttpStatusCode.ToString().MakeWord(),
                            Data = Data
                        },
                        Formatting.Indented//,
                        //new JsonSerializerSettings{
                        //    ReferenceLoopHandling = ReferenceLoopHandling.Serialize//,
                        //    PreserveReferencesHandling = PreserveReferencesHandling.None
                        //}
                    ),
                    Encoding.UTF8,
                    "application/json"
                );
            }
            else
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new Result()
                {
                    StatusCode = (int)HttpStatusCode,
                    StatusMessage = HttpStatusCode.ToString().MakeWord()
                }), Encoding.UTF8, "application/json");
            }

            response.RequestMessage = Request;
            return response;
        }

        private class Result
        {
            [JsonProperty(Order = 1)]
            public int StatusCode { get; set; }

            [JsonProperty(Order = 2)]
            public string StatusMessage { get; set; }
        }

        private class ResultWithData : Result
        {
            [JsonProperty(Order = 3)]
            public object Data { get; set; }
        }
    }
}