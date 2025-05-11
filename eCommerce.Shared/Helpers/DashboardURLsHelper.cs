using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eCommerce.Shared.Helpers
{
    public static class DashboardURLsHelper
    {
        public static string Dashboard(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_Dashboard");
            }
            else routeURL = helper.RouteUrl("Dashboard");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ListAction(this UrlHelper helper, string controller, string searchTerm = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        
        public static string Categories(this UrlHelper helper, string searchTerm = "", int? pageNo = 0, int? parentCategoryID = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Categories");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (parentCategoryID.HasValue && parentCategoryID.Value > 0)
            {
                routeValues.Add("parentCategoryID", parentCategoryID.Value);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Products(this UrlHelper helper, string searchTerm = "", int? pageNo = 0, int? categoryID = 0, bool? showOnlyLowStock = false)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Products");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (categoryID.HasValue && categoryID.Value > 0)
            {
                routeValues.Add("categoryID", categoryID.Value);
            }

            if (showOnlyLowStock.HasValue && showOnlyLowStock.Value)
            {
                routeValues.Add("showOnlyLowStock", showOnlyLowStock.Value);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string Promos(this UrlHelper helper, string searchTerm = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Promos");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }
            
            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        
        public static string Orders(this UrlHelper helper, string userID = "", int? orderID = 0, int? orderStatus = 0, int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Orders");

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
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UserOrdersList(this UrlHelper helper, string userID, int? orderID = 0, int? orderStatus = 0, int? pageNo = 0)
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
                routeURL = helper.RouteUrl("LanguageBased_UserOrdersList", routeValues);
            }
            else routeURL = helper.RouteUrl("UserOrdersList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string Languages(this UrlHelper helper, string searchTerm = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdateOrderStatus(this UrlHelper helper, int orderID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("orderID", orderID);
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UpdateOrderStatus", routeValues);
            }
            else routeURL = helper.RouteUrl("UpdateOrderStatus", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Users(this UrlHelper helper, string searchTerm = "", string roleID = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Users");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (!string.IsNullOrEmpty(roleID))
            {
                routeValues.Add("roleID", roleID);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }
            
            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Comments(this UrlHelper helper, string searchTerm = "", string userID = "", int? pageNo = 0, bool showUserCommentsOnly = false)
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

            if (showUserCommentsOnly)
            {
                routeValues.Add("showUserCommentsOnly", showUserCommentsOnly);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Configurations(this UrlHelper helper, string searchTerm = "", int? configurationType = 0, int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Configurations");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (configurationType.HasValue && configurationType.Value > 0)
            {
                routeValues.Add("configurationType", configurationType.Value);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityList", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityList", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string CreateAction(this UrlHelper helper, string controller)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityCreate", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityCreate", routeValues);
            
            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string EditAction(this UrlHelper helper, string controller, object ID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);
            routeValues.Add("ID", ID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityEdit", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityEdit", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DetailsAction(this UrlHelper helper, string controller, object ID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);
            routeValues.Add("ID", ID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityDetails", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityDetails", routeValues);
            
            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string EditAction(this UrlHelper helper, string controller)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityEditWithoutID", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityEditWithoutID", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DeleteAction(this UrlHelper helper, string controller)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", controller);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_EntityDelete", routeValues);
            }
            else routeURL = helper.RouteUrl("EntityDelete", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string DisableLanguage(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");
            routeValues.Add("disable", true);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ChangeLanguageStatus", routeValues);
            }
            else routeURL = helper.RouteUrl("ChangeLanguageStatus", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
        public static string EnableLanguage(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");
            routeValues.Add("disable", false);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ChangeLanguageStatus", routeValues);
            }
            else routeURL = helper.RouteUrl("ChangeLanguageStatus", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string LanguageResources(this UrlHelper helper, int ID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");
            routeValues.Add("Action", "Resources");
            routeValues.Add("ID", ID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_LanguageResources", routeValues);
            }
            else routeURL = helper.RouteUrl("LanguageResources", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ImportResources(this UrlHelper helper, int ID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");
            routeValues.Add("Action", "ImportResources");
            routeValues.Add("ID", ID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ImportLanguageResources", routeValues);
            }
            else routeURL = helper.RouteUrl("ImportLanguageResources", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ExportResources(this UrlHelper helper, int ID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("Controller", "Languages");
            routeValues.Add("Action", "ExportResources");
            routeValues.Add("ID", ID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_ExportLanguageResources", routeValues);
            }
            else routeURL = helper.RouteUrl("ExportLanguageResources", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UserDetails(this UrlHelper helper, string userID)
        {
            string routeURL = string.Empty;
            var routeValues = new RouteValueDictionary();

            routeValues.Add("userID", userID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UserDetails", routeValues);
            }
            else routeURL = helper.RouteUrl("UserDetails", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UserRoles(this UrlHelper helper, string userID)
        {
            string routeURL = string.Empty;
            var routeValues = new RouteValueDictionary();

            routeValues.Add("userID", userID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_UserRoles", routeValues);
            }
            else routeURL = helper.RouteUrl("UserRoles", routeValues);
            
            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string RoleDetails(this UrlHelper helper, string roleID)
        {
            string routeURL = string.Empty;
            var routeValues = new RouteValueDictionary();

            routeValues.Add("roleID", roleID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_RoleDetails", routeValues);
            }
            else routeURL = helper.RouteUrl("RoleDetails", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string RoleUsers(this UrlHelper helper, string roleID, int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("roleID", roleID);
            
            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_RoleUsers", routeValues);
            }
            else routeURL = helper.RouteUrl("RoleUsers", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string AssignUserRole(this UrlHelper helper, string userID)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            routeValues.Add("userID", userID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_AssignUserRole", routeValues);
            }
            else routeURL = helper.RouteUrl("AssignUserRole", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string RemoveUserRole(this UrlHelper helper, string userID)
        {
            string routeURL = string.Empty;
            var routeValues = new RouteValueDictionary();

            routeValues.Add("userID", userID);

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_RemoveUserRole", routeValues);
            }
            else routeURL = helper.RouteUrl("RemoveUserRole", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string NewsletterSubscribers(this UrlHelper helper, string searchTerm = "", int? pageNo = 0)
        {
            string routeURL = string.Empty;

            var routeValues = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                routeValues.Add("searchTerm", searchTerm);
            }

            if (pageNo.HasValue && pageNo.Value > 1)
            {
                routeValues.Add("pageNo", pageNo.Value);
            }

            if (ConfigurationsHelper.EnableMultilingual)
            {
                routeURL = helper.RouteUrl("LanguageBased_NewsletterSubscribers", routeValues);
            }
            else routeURL = helper.RouteUrl("NewsletterSubscribers", routeValues);

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }
    }
}