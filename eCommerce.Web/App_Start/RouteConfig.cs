using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eCommerce.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { area = "", controller = "Users", action = "Register" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "LanguageBased_Register",
                url: "{lang}/register",
                defaults: new { area = "", controller = "Users", action = "Register" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { area = "", controller = "Users", action = "Login" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "LanguageBased_Login",
                url: "{lang}/login",
                defaults: new { area = "", controller = "Users", action = "Login" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SocialLogin",
                url: "social-login",
                defaults: new { area = "", controller = "Users", action = "SocialLogin" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "LanguageBased_SocialLogin",
                url: "{lang}/social-login",
                defaults: new { area = "", controller = "Users", action = "SocialLogin" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SocialLoginCallback",
                url: "social-login-callback",
                defaults: new { area = "", controller = "Users", action = "SocialLoginCallback" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_SocialLoginCallback",
                url: "{lang}/social-login-callback",
                defaults: new { area = "", controller = "Users", action = "SocialLoginCallback" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "forgot-password",
                defaults: new { area = "", controller = "Users", action = "ForgotPassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "LanguageBased_ForgotPassword",
                url: "{lang}/forgot-password",
                defaults: new { area = "", controller = "Users", action = "ForgotPassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "reset-password",
                defaults: new { area = "", controller = "Users", action = "ResetPassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "LanguageBased_ResetPassword",
                url: "{lang}/reset-password",
                defaults: new { area = "", controller = "Users", action = "ResetPassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Logoff",
                url: "logoff",
                defaults: new { area = "", controller = "Users", action = "LogOff" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_Logoff",
                url: "{lang}/logoff",
                defaults: new { area = "", controller = "Users", action = "LogOff" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "AboutUs",
                url: "about-us",
                defaults: new { area = "", controller = "Contents", action = "AboutUs" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_AboutUs",
                url: "{lang}/about-us",
                defaults: new { area = "", controller = "Contents", action = "AboutUs" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ContactUs",
                url: "contact-us",
                defaults: new { area = "", controller = "Contents", action = "ContactUs" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ContactUs",
                url: "{lang}/contact-us",
                defaults: new { area = "", controller = "Contents", action = "ContactUs" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Blog",
                url: "blog",
                defaults: new { area = "", controller = "Contents", action = "Blog" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_Blog",
                url: "{lang}/blog",
                defaults: new { area = "", controller = "Contents", action = "Blog" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PrivacyPolicy",
                url: "privacy-policy",
                defaults: new { area = "", controller = "Contents", action = "PrivacyPolicy" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_PrivacyPolicy",
                url: "{lang}/privacy-policy",
                defaults: new { area = "", controller = "Contents", action = "PrivacyPolicy" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RefundPolicy",
                url: "refund-policy",
                defaults: new { area = "", controller = "Contents", action = "RefundPolicy" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_RefundPolicy",
                url: "{lang}/refund-policy",
                defaults: new { area = "", controller = "Contents", action = "RefundPolicy" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "TermsConditions",
                url: "terms-conditions",
                defaults: new { area = "", controller = "Contents", action = "TermsConditions" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_TermsConditions",
                url: "{lang}/terms-conditions",
                defaults: new { area = "", controller = "Contents", action = "TermsConditions" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SearchProducts",
                url: "search/{category}",
                defaults: new { area = "", controller = "Home", action = "Search", category = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_SearchProducts",
                url: "{lang}/search/{category}",
                defaults: new { area = "", controller = "Home", action = "Search", category = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ProductDetails",
                url: "{category}/product/{ID}/{sanitizedtitle}",
                defaults: new { area = "", controller = "Products", action = "Details", sanitizedtitle = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ProductDetails",
                url: "{lang}/{category}/product/{ID}/{sanitizedtitle}",
                defaults: new { area = "", controller = "Products", action = "Details", sanitizedtitle = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UserProfile",
                url: "user/profile",
                defaults: new { area = "", controller = "Users", action = "UserProfile" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UserProfile",
                url: "{lang}/user/profile",
                defaults: new { area = "", controller = "Users", action = "UserProfile" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UpdateProfile",
                url: "user/update-profile",
                defaults: new { area = "", controller = "Users", action = "UpdateProfile" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UpdateProfile",
                url: "{lang}/user/update-profile",
                defaults: new { area = "", controller = "Users", action = "UpdateProfile" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ChangePassword",
                url: "user/change-password",
                defaults: new { area = "", controller = "Users", action = "ChangePassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ChangePassword",
                url: "{lang}/user/change-password",
                defaults: new { area = "", controller = "Users", action = "ChangePassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UpdatePassword",
                url: "user/update-password",
                defaults: new { area = "", controller = "Users", action = "UpdatePassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UpdatePassword",
                url: "{lang}/user/update-password",
                defaults: new { area = "", controller = "Users", action = "UpdatePassword" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ChangeAvatar",
                url: "user/change-avatar",
                defaults: new { area = "", controller = "Users", action = "ChangeAvatar" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ChangeAvatar",
                url: "{lang}/user/change-avatar",
                defaults: new { area = "", controller = "Users", action = "ChangeAvatar" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UpdateAvatar",
                url: "user/update-avatar",
                defaults: new { area = "", controller = "Users", action = "UpdateAvatar" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UpdateAvatar",
                url: "{lang}/user/update-avatar",
                defaults: new { area = "", controller = "Users", action = "UpdateAvatar" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UserOrders",
                url: "user/orders",
                defaults: new { controller = "Orders", action = "UserOrders" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UserOrders",
                url: "{lang}/user/orders",
                defaults: new { controller = "Orders", action = "UserOrders" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Cart",
                url: "cart",
                defaults: new { area = "", controller = "Cart", action = "Cart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_Cart",
                url: "{lang}/cart",
                defaults: new { area = "", controller = "Cart", action = "Cart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UpdateCart",
                url: "update-cart",
                defaults: new { area = "", controller = "Cart", action = "UpdateCart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UpdateCart",
                url: "{lang}/update-cart",
                defaults: new { area = "", controller = "Cart", action = "UpdateCart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "AddItemToCart",
                url: "add-to-cart",
                defaults: new { area = "", controller = "Cart", action = "AddItemToCart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_AddItemToCart",
                url: "{lang}/add-to-cart",
                defaults: new { area = "", controller = "Cart", action = "AddItemToCart" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "GetCartItems",
                url: "get-cart-items",
                defaults: new { area = "", controller = "Cart", action = "GetCartItems" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_GetCartItems",
                url: "{lang}/get-cart-items",
                defaults: new { area = "", controller = "Cart", action = "GetCartItems" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "checkout",
                defaults: new { area = "", controller = "Cart", action = "Checkout" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_Checkout",
                url: "{lang}/checkout",
                defaults: new { area = "", controller = "Cart", action = "Checkout" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "DeliveryInfo",
                url: "delivery-info",
                defaults: new { area = "", controller = "Cart", action = "DeliveryInfo" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_DeliveryInfo",
                url: "{lang}/delivery-info",
                defaults: new { area = "", controller = "Cart", action = "DeliveryInfo" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ConfirmOrder",
                url: "confirm-order",
                defaults: new { area = "", controller = "Cart", action = "ConfirmOrder" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ConfirmOrder",
                url: "{lang}/confirm-order",
                defaults: new { area = "", controller = "Cart", action = "ConfirmOrder" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PlaceOrder",
                url: "place-order",
                defaults: new { area = "", controller = "Cart", action = "PlaceOrder" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_PlaceOrder",
                url: "{lang}/place-order",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrder" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PlaceOrderViaCashOnDelivery",
                url: "place-order-cod",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrderViaCashOnDelivery" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_PlaceOrderViaCashOnDelivery",
                url: "{lang}/place-order-cod",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrderViaCashOnDelivery" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PlaceOrderViaPayPal",
                url: "place-order-paypal",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrderViaPayPal" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_PlaceOrderViaPayPal",
                url: "{lang}/place-order-paypal",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrderViaPayPal" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "OrderTrack",
                url: "tracking",
                defaults: new { area = "", controller = "Orders", action = "Tracking" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_OrderTrack",
                url: "{lang}/tracking",
                defaults: new { area = "", controller = "Orders", action = "Tracking" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "PrintInvoice",
                url: "print-ivoice",
                defaults: new { area = "", controller = "Orders", action = "PrintInvoice" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_PrintInvoice",
                url: "{lang}/print-ivoice",
                defaults: new { area = "", controller = "Orders", action = "PrintInvoice" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "FeaturedProducts",
                url: "featured-products",
                defaults: new { area = "", controller = "Products", action = "FeaturedProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_FeaturedProducts",
                url: "{lang}/featured-products",
                defaults: new { area = "", controller = "Products", action = "FeaturedProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RecentProducts",
                url: "recent-products",
                defaults: new { area = "", controller = "Products", action = "RecentProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_RecentProducts",
                url: "{lang}/recent-products",
                defaults: new { area = "", controller = "Products", action = "RecentProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RelatedProducts",
                url: "related-products",
                defaults: new { area = "", controller = "Products", action = "RelatedProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_RelatedProducts",
                url: "{lang}/related-products",
                defaults: new { area = "", controller = "Products", action = "RelatedProducts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "CategoriesMenu",
                url: "categories-menu",
                defaults: new { area = "", controller = "Categories", action = "CategoriesMenu" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_CategoriesMenu",
                url: "{lang}/categories-menu",
                defaults: new { area = "", controller = "Categories", action = "CategoriesMenu" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UploadPictures",
                url: "pictures/upload",
                defaults: new { area = "", controller = "Shared", action = "UploadPictures" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UploadPictures",
                url: "{lang}/pictures/upload",
                defaults: new { area = "", controller = "Shared", action = "UploadPictures" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );


            routes.MapRoute(
                name: "UploadPicturesWithoutDatabase",
                url: "pictures/upload-nodb/{subFolder}",
                defaults: new { area = "", controller = "Shared", action = "UploadPicturesWithoutDatabase", subFolder = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UploadPicturesWithoutDatabase",
                url: "{lang}/pictures/upload-nodb/{subFolder}",
                defaults: new { area = "", controller = "Shared", action = "UploadPicturesWithoutDatabase", subFolder = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );


            routes.MapRoute(
                name: "LeaveComment",
                url: "add-comment",
                defaults: new { area = "", controller = "Comments", action = "LeaveComment" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_LeaveComment",
                url: "{lang}/add-comment",
                defaults: new { area = "", controller = "Comments", action = "LeaveComment" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "DeleteComment",
                url: "delete-comment",
                defaults: new { area = "", controller = "Comments", action = "DeleteComment" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_DeleteComment",
                url: "{lang}/delete-comment",
                defaults: new { area = "", controller = "Comments", action = "DeleteComment" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "UserComments",
                url: "user/comments",
                defaults: new { area = "", controller = "Comments", action = "UserComments" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_UserComments",
                url: "{lang}/user/comments",
                defaults: new { area = "", controller = "Comments", action = "UserComments" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SubscribeNewsLetter",
                url: "newsletter-subscription",
                defaults: new { area = "", controller = "Home", action = "SubscribeNewsLetter" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_SubscribeNewsLetter",
                url: "{lang}/newsletter-subscription",
                defaults: new { area = "", controller = "Home", action = "SubscribeNewsLetter" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            
            routes.MapRoute(
                name: "SubmitContactForm",
                url: "contact-form-submit",
                defaults: new { area = "", controller = "Home", action = "SubmitContactForm" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_SubmitContactForm",
                url: "{lang}/contact-form-submit",
                defaults: new { area = "", controller = "Home", action = "SubmitContactForm" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
            
            routes.MapRoute(
                name: "ChangeMode",
                url: "change-mode",
                defaults: new { area = "", controller = "Shared", action = "ChangeMode" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ChangeMode",
                url: "{lang}/change-mode",
                defaults: new { area = "", controller = "Shared", action = "ChangeMode" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ExternalSocialScripts",
                url: "external-social-scripts",
                defaults: new { area = "", controller = "Shared", action = "ExternalSocialScripts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_ExternalSocialScripts",
                url: "{lang}/external-social-scripts",
                defaults: new { area = "", controller = "Shared", action = "ExternalSocialScripts" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "LanguageBased_Home",
                url: "{lang}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "eCommerce.Web.Controllers" }
            );
        }
    }
}
