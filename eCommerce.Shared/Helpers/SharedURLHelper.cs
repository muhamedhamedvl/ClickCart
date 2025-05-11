using eCommerce.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace eCommerce.Shared.Helpers
{
    public static class SharedURLHelper
    {
        #region GetUrlComponentMethods
        public static string GetLanguageUrlComponent()
        {
            var languageShortName = GetUrlValueByKey("lang");

            //return !string.IsNullOrEmpty(languageShortName) ? languageShortName : "en";
            return languageShortName;
        }
        public static string GetUrlComponentByKey(string key)
        {
            return GetUrlValueByKey(key);
        }

        private static string GetUrlValueByKey(string key)
        {
            RouteValueDictionary routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            string value = string.Empty;
            foreach (var routeValue in routeValues)
            {
                if (routeValue.Key.ToLower() == key.ToLower())
                {
                    value = routeValue.Value.ToString();
                    break;
                }
            }

            return value;
        }

        public static bool TryAddRouteKeyValue(string key, string value, bool forceUpdate = false)
        {
            RouteValueDictionary routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (!routeValues.ContainsKey(key))
            {
                routeValues.Add(key, value);

                return true;
            }
            else
            {
                if(forceUpdate)
                {
                    routeValues[key] = value;
                }

                return false;
            }
        }
        #endregion

        public static string LanguageBasedURL(this UrlHelper helper, string langShortCode)
        {
            Route currentRoute = (Route)HttpContext.Current.Request.RequestContext.RouteData.Route;

            string routeURL = currentRoute.Url.ToLower().Trim();

            string queryString = HttpContext.Current.Request.QueryString.ToString() == string.Empty ? string.Empty : "?" + HttpContext.Current.Request.QueryString;

            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            foreach (var routeValue in routeValues)
            {
                if (routeValue.Key.Equals("lang"))
                {
                    routeURL = routeURL.Replace(string.Format("{{{0}}}", routeValue.Key.ToLower().Trim()), langShortCode);
                }
                else
                {
                    routeURL = routeURL.Replace(string.Format("{{{0}}}", routeValue.Key.ToLower().Trim()), routeValue.Value.ToString().Trim());
                }
            }

            routeURL = !string.IsNullOrEmpty(routeURL) ? routeURL.ReplaceUnpassedRouteValues() : string.Empty;

            return string.Format("{0}/{1}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), routeURL + queryString).ToLower();
        }
    }
}
