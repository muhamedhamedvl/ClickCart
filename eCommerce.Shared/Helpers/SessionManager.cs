using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eCommerce.Shared.Helpers
{
    public class SessionManager
    {
        private static SessionStorageType sessionStorage = SessionStorageType.NotSpecified;

        public static SessionStorageType SessionStorage {
            get {
                return sessionStorage;
            }
            set {
                sessionStorage = value;
            }
        }

        private static void CheckSessionStorage()
        {
            if (sessionStorage == SessionStorageType.NotSpecified)
            {
                string sessionStoreage = "httpsession";
                if (string.IsNullOrEmpty(sessionStoreage))
                {
                    sessionStorage = SessionStorageType.HTTPSession;
                }
                else
                {
                    sessionStoreage = sessionStoreage.ToLower();
                    switch (sessionStoreage)
                    {
                        case "httpsession":
                            sessionStorage = SessionStorageType.HTTPSession;
                            break;
                        case "cookie":
                            sessionStorage = SessionStorageType.Cookie;
                            break;
                        default:
                            sessionStorage = SessionStorageType.HTTPSession;
                            break;
                    }
                }
            }
        }

        public static T Get<T>(string aKey)
        {
            T retVal = default(T);
            try
            {
                CheckSessionStorage();
                switch (sessionStorage)
                {
                    case SessionStorageType.HTTPSession:
                        retVal = (T)HttpContext.Current.Session[aKey];
                        break;
                    case SessionStorageType.Cookie:
                        retVal = CookiesHelper.GetObjectFromCookie<T>(aKey);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retVal;
        }
        public static void Set<T>(string aKey, T aValue)
        {
            CheckSessionStorage();

            switch (sessionStorage)
            {
                case SessionStorageType.HTTPSession:
                    HttpContext.Current.Session[aKey] = aValue;
                    break;
                case SessionStorageType.Cookie:
                    //CookieHelper.SetObjectToCookie(aKey, aValue);
                    break;
            }
        }
        public static void ClearKey(string aKey)
        {
            CheckSessionStorage();

            switch (sessionStorage)
            {
                case SessionStorageType.HTTPSession:
                    HttpContext.Current.Session[aKey] = null;
                    break;
                case SessionStorageType.Cookie:
                    //CookieHelper.SetObjectToCookie(aKey, "");
                    break;
            }
        }
    }

    public enum SessionStorageType : int
    {
        NotSpecified = 1,
        HTTPSession = 2,
        Cookie = 3
    }
}
