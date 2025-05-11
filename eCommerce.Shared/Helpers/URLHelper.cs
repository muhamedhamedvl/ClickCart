using eCommerce.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eCommerce.Shared.Helpers
{
    public static class URLHelper
    {
        public static string Home(this UrlHelper helper)
        {
            var routeValues = new RouteValueDictionary();

            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Home");
            }
            else routeURL = helper.RouteUrl("Home");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string StaticPage(this UrlHelper helper, string pageid)
        {
            var routeValues = new RouteValueDictionary();

            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl(string.Format("LanguageBased_{0}", pageid));
            }
            else routeURL = helper.RouteUrl(string.Format("{0}", pageid));

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string SubscribeNewsLetter(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_SubscribeNewsLetter");
            }
            else routeURL = helper.RouteUrl("SubscribeNewsLetter");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string SubmitContactForm(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_SubmitContactForm");
            }
            else routeURL = helper.RouteUrl("SubmitContactForm");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Register(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Register");
            }
            else routeURL = helper.RouteUrl("Register");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Login(this UrlHelper helper, string returnUrl = "")
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            if(!string.IsNullOrEmpty(returnUrl))
            {
                routeValues.Add("returnUrl", returnUrl);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Login", routeValues);
            }
            else routeURL = helper.RouteUrl("Login", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string SocialLoginCallback(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_SocialLoginCallback");
            }
            else routeURL = helper.RouteUrl("SocialLoginCallback");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ForgotPassword(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ForgotPassword");
            }
            else routeURL = helper.RouteUrl("ForgotPassword");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ResetPassword(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ResetPassword");
            }
            else routeURL = helper.RouteUrl("ResetPassword");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Logoff(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Logoff");
            }
            else routeURL = helper.RouteUrl("Logoff");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string SearchProducts(this UrlHelper helper, string category = "", string q = "", decimal? from = 0.0M, decimal? to = 0.0M, string sortby = "", int? pageNo = 0, int? recordSize = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("category", category);

            if (!string.IsNullOrEmpty(q))
            {
                routeValues.Add("q", q);
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                routeValues.Add("from", from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                routeValues.Add("to", to.Value);
            }

            if (!string.IsNullOrEmpty(sortby))
            {
                routeValues.Add("sortby", sortby);
            }

            if (recordSize.HasValue && recordSize.Value > 1 && recordSize.Value != (int)RecordSizeEnums.Size20)
            {
                routeValues.Add("recordSize", recordSize.Value);
            }
            
            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_SearchProducts", routeValues);
            }
            else routeURL = helper.RouteUrl("SearchProducts", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ProductDetails(this UrlHelper helper, string category, int ID, string sanitizedtitle = "")
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("category", category);
            routeValues.Add("ID", ID);

            if(!string.IsNullOrEmpty(sanitizedtitle))
            {
                routeValues.Add("sanitizedtitle", sanitizedtitle);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ProductDetails", routeValues);
            }
            else routeURL = helper.RouteUrl("ProductDetails", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UserProfile(this UrlHelper helper, string tab = "")
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            if(!string.IsNullOrEmpty(tab))
            {
                routeValues.Add("tab", tab);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UserProfile", routeValues);
            }
            else routeURL = helper.RouteUrl("UserProfile", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string UpdateProfile(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UpdateProfile");
            }
            else routeURL = helper.RouteUrl("UpdateProfile");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ChangePassword(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ChangePassword");
            }
            else routeURL = helper.RouteUrl("ChangePassword");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string UpdatePassword(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UpdatePassword");
            }
            else routeURL = helper.RouteUrl("UpdatePassword");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ChangeAvatar(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ChangeAvatar");
            }
            else routeURL = helper.RouteUrl("ChangeAvatar");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string UpdateAvatar(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UpdateAvatar");
            }
            else routeURL = helper.RouteUrl("UpdateAvatar");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        
        public static string UserOrders(this UrlHelper helper, string userID = "", int? orderID = 0, int? orderStatus = 0, int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(userID))
            {
                routeValues.Add("userID", userID);
            }

            if (orderID.HasValue && orderID.Value > 0)
            {
                routeValues.Add("orderID", orderID.Value);
            }

            if (orderStatus.HasValue && orderStatus.Value > 0)
            {
                routeValues.Add("orderStatus", orderStatus.Value);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UserOrders", routeValues);
            }
            else routeURL = helper.RouteUrl("UserOrders", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Cart(this UrlHelper helper, bool isPopup = false)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            if (isPopup)
            {
                routeValues.Add("isPopup", isPopup);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Cart", routeValues);
            }
            else routeURL = helper.RouteUrl("Cart", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string UpdateCart(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UpdateCart");
            }
            else routeURL = helper.RouteUrl("UpdateCart");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string AddItemToCart(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_AddItemToCart");
            }
            else routeURL = helper.RouteUrl("AddItemToCart");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string GetCartItems(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_GetCartItems");
            }
            else routeURL = helper.RouteUrl("GetCartItems");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string CartProducts(this UrlHelper helper)
        {
            string routeURL = string.Empty;
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_CartProducts");
            }
            else routeURL = helper.RouteUrl("CartProducts");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string CartItems(this UrlHelper helper)
        {
            string routeURL = string.Empty;
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_CartItems");
            }
            else routeURL = helper.RouteUrl("CartItems");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string Checkout(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Checkout");
            }
            else routeURL = helper.RouteUrl("Checkout");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string PlaceOrder(this UrlHelper helper, bool isCashOnDelivery = false, bool isPayPal = false)
        {
            string routeURL = string.Empty;
            var LanguageBased = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                LanguageBased = "LanguageBased_";
            }

            if (isCashOnDelivery)
            {
                routeURL = helper.RouteUrl(string.Format("{0}PlaceOrderViaCashOnDelivery", LanguageBased));
            }
            else if (isPayPal)
            {
                routeURL = helper.RouteUrl(string.Format("{0}PlaceOrderViaPayPal", LanguageBased));
            }
            else
            {
                routeURL = helper.RouteUrl(string.Format("{0}PlaceOrder", LanguageBased));
            }

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string OrderTrack(this UrlHelper helper, string orderID = "", bool orderPlaced = false)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();
            
            if (!string.IsNullOrEmpty(orderID))
            {
                routeValues.Add("orderID", orderID);
            }

            if (orderPlaced)
            {
                routeValues.Add("orderPlaced", orderPlaced);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_OrderTrack", routeValues);
            }
            else routeURL = helper.RouteUrl("OrderTrack", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string PrintInvoice(this UrlHelper helper, int orderID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("orderID", orderID);
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_PrintInvoice", routeValues);
            }
            else routeURL = helper.RouteUrl("PrintInvoice", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UploadPictures(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UploadPictures");
            }
            else routeURL = helper.RouteUrl("UploadPictures");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UploadPicturesWithoutDatabase(this UrlHelper helper, bool isSiteFolder = false, string subFolder = "")
        {
            string routeURL = string.Empty;
            var routeValues = new RouteValueDictionary();
            
            if (isSiteFolder)
            {
                routeValues.Add("isSiteFolder", isSiteFolder);
            }

            if (!string.IsNullOrEmpty(subFolder))
            {
                routeValues.Add("subFolder", subFolder);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UploadPicturesWithoutDatabase", routeValues);
            }
            else routeURL = helper.RouteUrl("UploadPicturesWithoutDatabase", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string LeaveComment(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_LeaveComment");
            }
            else routeURL = helper.RouteUrl("LeaveComment");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DeleteComment(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_DeleteComment");
            }
            else routeURL = helper.RouteUrl("DeleteComment");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string UserComments(this UrlHelper helper, string userID = "", string searchTerm = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Comments");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                routeValues.Add("userID", userID);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UserComments", routeValues);
            }
            else routeURL = helper.RouteUrl("UserComments", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ChangeMode(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ChangeMode");
            }
            else routeURL = helper.RouteUrl("ChangeMode");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ExternalSocialScripts(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ExternalSocialScripts");
            }
            else routeURL = helper.RouteUrl("ExternalSocialScripts");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
    }
}