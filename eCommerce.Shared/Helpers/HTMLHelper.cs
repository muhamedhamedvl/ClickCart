using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Shared.Helpers
{
    public static class HTMLHelper
    {
        public static MvcHtmlString MenuItemClass(this HtmlHelper htmlHelper, string controllerName, string actionName = "")
        {
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            if (String.Equals(controllerName, currentController, StringComparison.CurrentCultureIgnoreCase))
            {
                if(!string.IsNullOrEmpty(actionName))
                {
                    var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");

                    if (String.Equals(actionName, currentAction, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return new MvcHtmlString("active");
                    }
                    else return new MvcHtmlString("");
                }
                else return new MvcHtmlString("active");
            }
            else
                return new MvcHtmlString("");
        }

        public static string getCellBackgroundClassByOrderStatus(this HtmlHelper htmlHelper, OrderStatus orderStatus)
        {
            var bgClass = string.Empty;

            switch (orderStatus)
            {
                case OrderStatus.Placed:
                    bgClass = "bg-primary text-white";
                    break;
                case OrderStatus.Processing:
                case OrderStatus.WaitingForPayment:
                    bgClass = "bg-info text-white";
                    break;
                case OrderStatus.Delivered:
                    bgClass = "bg-success text-white";
                    break;
                case OrderStatus.Failed:
                case OrderStatus.Cancelled:
                    bgClass = "bg-danger text-white";
                    break;
                case OrderStatus.OnHold:
                case OrderStatus.Refunded:
                    bgClass = "bg-warning";
                    break;
                default:
                    bgClass = string.Empty;
                    break;
            }

            return bgClass;
        }
        public static string getCellBackgroundClassByLanguageStatus(this HtmlHelper htmlHelper, bool enabled)
        {
            var bgClass = string.Empty;

            if(!enabled)
            {
                bgClass = "bg-danger text-white";
            }
            else
            {
                bgClass = "bg-success text-white";
            }
            
            return bgClass;
        }

        public static string GetFontAwesomeIconForSocialMediaProvider(this HtmlHelper htmlHelper, string socialMediaProvider)
        {
            var fontAwesomeClass = string.Empty;

            switch (socialMediaProvider)
            {
                case "Facebook":
                    fontAwesomeClass = "fab fa-facebook-f";
                    break;
                case "Twitter":
                    fontAwesomeClass = "fab fa-twitter";
                    break;
                case "Google":
                    fontAwesomeClass = "fab fa-google";
                    break;
                case "Microsoft":
                    fontAwesomeClass = "fab fa-microsoft";
                    break;
                default:
                    fontAwesomeClass = string.Empty;
                    break;
            }

            return fontAwesomeClass;
        }
        public static string GetButtonBackgroundClassForSocialMediaProvider(this HtmlHelper htmlHelper, string socialMediaProvider)
        {
            var fontAwesomeClass = string.Empty;

            switch (socialMediaProvider)
            {
                case "Facebook":
                    fontAwesomeClass = "bg-primary";
                    break;
                case "Twitter":
                    fontAwesomeClass = "bg-info";
                    break;
                case "Google":
                    fontAwesomeClass = "bg-danger";
                    break;
                case "Microsoft":
                    fontAwesomeClass = "bg-success";
                    break;
                default:
                    fontAwesomeClass = string.Empty;
                    break;
            }

            return fontAwesomeClass;
        }
    }
}