using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace eCommerce.Shared.Helpers
{
    public static class CookiesHelper
    {
        private const string COOKIE_NAME = "ObjValue";

        public static T GetObjectFromCookie<T>(string key)
        {
            T retVal = default(T);
            string strValue = GetStringFromCookie(key);
            if (strValue != "")
            {
                retVal = DeSerializeObject<T>(strValue);
            }
            return retVal;
        }

        private static string GetStringFromCookie(string key)
        {
            string retVal = "";
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[COOKIE_NAME];
            if (myCookie != null)
            {
                retVal = HttpUtility.UrlDecode(myCookie.Values[key]);
            }
            return retVal;
        }

        internal static string SerializeObject<T>(this T toSerialize)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(toSerialize);
        }

        internal static T DeSerializeObject<T>(string objValue)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(objValue);
        }

    }
}
