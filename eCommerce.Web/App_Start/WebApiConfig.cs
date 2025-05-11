using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace eCommerce.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_Search",
                routeTemplate: "api/{lang}/search/{category}",
                defaults: new { area = "API", controller = "Home", action = "Search", category = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_Sliders",
                routeTemplate: "api/{lang}/sliders",
                defaults: new { area = "API", controller = "Home", action = "Sliders" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_Configurations",
                routeTemplate: "api/{lang}/configurations",
                defaults: new { area = "API", controller = "Home", action = "Configurations" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_FeaturedCategories",
                routeTemplate: "api/{lang}/featured-categories",
                defaults: new { area = "API", controller = "Home", action = "FeaturedCategories" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_FeaturedProducts",
                routeTemplate: "api/{lang}/featured-products",
                defaults: new { area = "API", controller = "Home", action = "FeaturedProducts" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_ProductDetails",
                routeTemplate: "api/{lang}/product-details/{ID}",
                defaults: new { area = "API", controller = "Home", action = "ProductDetails" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_ProductRatings",
                routeTemplate: "api/{lang}/product-comments/{ID}",
                defaults: new { area = "API", controller = "Home", action = "ProductComments" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_RelatedProducts",
                routeTemplate: "api/{lang}/related-products/{ID}",
                defaults: new { area = "API", controller = "Home", action = "RelatedProducts" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_CategoriesMenu",
                routeTemplate: "api/{lang}/categories-menu",
                defaults: new { area = "API", controller = "Home", action = "CategoriesMenu" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_StoreInfo",
                routeTemplate: "api/{lang}/store-info",
                defaults: new { area = "API", controller = "Home", action = "StoreInfo" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_OrderTracking",
                routeTemplate: "api/{lang}/order-tracking/{orderID}",
                defaults: new { area = "API", controller = "Orders", action = "Tracking" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserRegister",
                routeTemplate: "api/{lang}/user/register",
                defaults: new { area = "API", controller = "Users", action = "Register" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserLogout",
                routeTemplate: "api/{lang}/user/logout",
                defaults: new { area = "API", controller = "Users", action = "Logout" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserInfo",
                routeTemplate: "api/{lang}/user/info",
                defaults: new { area = "API", controller = "Users", action = "UserInfo" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserInfo_Update",
                routeTemplate: "api/{lang}/user/info/update",
                defaults: new { area = "API", controller = "Users", action = "UpdateUserInfo" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserPassword_Update",
                routeTemplate: "api/{lang}/user/password/update",
                defaults: new { area = "API", controller = "Users", action = "UpdateUserPassword" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserAvatar_Update",
                routeTemplate: "api/{lang}/user/avatar/update",
                defaults: new { area = "API", controller = "Users", action = "UpdateUserAvatar" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserOrders",
                routeTemplate: "api/{lang}/user/orders",
                defaults: new { area = "API", controller = "Users", action = "UserOrders" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_UserComments",
                routeTemplate: "api/{lang}/user/comments",
                defaults: new { area = "API", controller = "Users", action = "UserComments" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_AddProductToCart",
                routeTemplate: "api/{lang}/add-to-cart/{ID}",
                defaults: new { area = "API", controller = "Cart", action = "AddProductToCart" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_ApplyPromoToCart",
                routeTemplate: "api/{lang}/apply-promo",
                defaults: new { area = "API", controller = "Cart", action = "ApplyPromoToCart" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_Cart",
                routeTemplate: "api/{lang}/cart",
                defaults: new { area = "API", controller = "Cart", action = "CartDetails" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_ClearCart",
                routeTemplate: "api/{lang}/clear-cart",
                defaults: new { area = "API", controller = "Cart", action = "ClearCart" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_PlaceOrderViaCashOnDelivery",
                routeTemplate: "api/{lang}/place-order-cod",
                defaults: new { area = "API", controller = "Orders", action = "PlaceOrderViaCashOnDelivery" }
            );

            config.Routes.MapHttpRoute(
                name: "API_LanguageBased_PlaceOrderCreditCard",
                routeTemplate: "api/{lang}/place-order-credit-card",
                defaults: new { area = "API", controller = "Orders", action = "PlaceOrderCreditCard" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultAPI",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { area = "API", controller = "APIBase", action = "Default", id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }
}
