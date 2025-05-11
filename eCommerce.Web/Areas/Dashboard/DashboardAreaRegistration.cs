using System.Web.Mvc;

namespace eCommerce.Web.Areas.dashboard
{
    public class DashboardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Dashboard",
                "dashboard",
                new { controller = "dashboard", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "LanguageBased_Dashboard",
                "{lang}/dashboard",
                new { controller = "dashboard", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "UnAuthorized",
                "dashboard/UnAuthorized",
                new { controller = "DashboardBase", action = "UnAuthorized" }
            );

            context.MapRoute(
                "LanguageBased_UnAuthorized",
                "{lang}/dashboard/UnAuthorized",
                new { controller = "DashboardBase", action = "UnAuthorized" }
            );

            context.MapRoute(
                name: "NewsletterSubscribers",
                url: "dashboard/newsletter-subscribers/",
                defaults: new { controller = "Dashboard", action = "NewsletterSubscribers" }
            );

            context.MapRoute(
                name: "LanguageBased_NewsletterSubscribers",
                url: "{lang}/dashboard/newsletter-subscribers/",
                defaults: new { controller = "Dashboard", action = "NewsletterSubscribers" }
            );

            context.MapRoute(
                "EntityList",
                "dashboard/{controller}/",
                new { action = "Index" }
            );

            context.MapRoute(
                "LanguageBased_EntityList",
                "{lang}/dashboard/{controller}/",
                new { action = "Index" }
            );

            context.MapRoute(
                "EntityCreate",
                "dashboard/{controller}/Create/",
                new { action = "Action" }
            );

            context.MapRoute(
                "LanguageBased_EntityCreate",
                "{lang}/dashboard/{controller}/Create/",
                new { action = "Action" }
            );

            context.MapRoute(
                "EntityEdit",
                "dashboard/{controller}/Edit/{id}",
                new { action = "Action" }
            );

            context.MapRoute(
                "LanguageBased_EntityEdit",
                "{lang}/dashboard/{controller}/Edit/{id}",
                new { action = "Action" }
            );

            context.MapRoute(
                "EntityEditWithoutID",
                "dashboard/{controller}/Edit/",
                new { action = "Action" }
            );

            context.MapRoute(
                "LanguageBased_EntityEditWithoutID",
                "{lang}/dashboard/{controller}/Edit/",
                new { action = "Action" }
            );

            context.MapRoute(
                "EntityDetails",
                "dashboard/{controller}/Details/{id}",
                new { action = "Details" }
            );

            context.MapRoute(
                "LanguageBased_EntityDetails",
                "{lang}/dashboard/{controller}/Details/{id}",
                new { action = "Details" }
            );

            context.MapRoute(
                "EntityDelete",
                "dashboard/{controller}/Delete/",
                new { action = "Delete" }
            );

            context.MapRoute(
                "LanguageBased_EntityDelete",
                "{lang}/dashboard/{controller}/Delete/",
                new { action = "Delete" }
            );

            context.MapRoute(
                "ChangeLanguageStatus",
                "dashboard/{controller}/change-status/",
                new { controller = "Languages", action = "ChangeLanguageStatus" }
            );

            context.MapRoute(
                "LanguageBased_ChangeLanguageStatus",
                "{lang}/dashboard/{controller}/change-status/",
                new { controller = "Languages", action = "ChangeLanguageStatus" }
            );
            
            context.MapRoute(
                "LanguageResources",
                "dashboard/Languages/Resources/{id}",
                new { controller = "Languages", action = "Resources" }
            );

            context.MapRoute(
                "LanguageBased_LanguageResources",
                "{lang}/dashboard/Languages/Resources/{id}",
                new { controller = "Languages", action = "Resources" }
            );

            context.MapRoute(
                "ImportLanguageResources",
                "dashboard/Languages/Resources/import/{id}",
                new { controller = "Languages", action = "ImportResources" }
            );

            context.MapRoute(
                "LanguageBased_ImportLanguageResources",
                "{lang}/dashboard/Languages/Resources/import/{id}",
                new { controller = "Languages", action = "ImportResources" }
            );

            context.MapRoute(
                "ExportLanguageResources",
                "dashboard/Languages/Resources/export/{id}",
                new { controller = "Languages", action = "ExportResources" }
            );

            context.MapRoute(
                "LanguageBased_ExportLanguageResources",
                "{lang}/dashboard/Languages/Resources/export/{id}",
                new { controller = "Languages", action = "ExportResources" }
            );

            context.MapRoute(
                name: "UserDetails",
                url: "dashboard/Users/UserDetails/{userID}",
                defaults: new { controller = "Users", action = "UserDetails" }
            );

            context.MapRoute(
                name: "LanguageBased_UserDetails",
                url: "{lang}/dashboard/Users/UserDetails/{userID}",
                defaults: new { controller = "Users", action = "UserDetails" }
            );

            context.MapRoute(
                name: "UserRoles",
                url: "dashboard/Users/UserRoles/{userID}",
                defaults: new { controller = "Roles", action = "UserRoles" }
            );

            context.MapRoute(
                name: "LanguageBased_UserRoles",
                url: "{lang}/dashboard/Users/UserRoles/{userID}",
                defaults: new { controller = "Roles", action = "UserRoles" }
            );

            context.MapRoute(
                name: "RoleDetails",
                url: "dashboard/Roles/RoleDetails/{roleID}",
                defaults: new { controller = "Roles", action = "RoleDetails" }
            );

            context.MapRoute(
                name: "LanguageBased_RoleDetails",
                url: "{lang}/dashboard/Roles/RoleDetails/{roleID}",
                defaults: new { controller = "Roles", action = "RoleDetails" }
            );

            context.MapRoute(
                name: "RoleUsers",
                url: "dashboard/Roles/RoleUsers/{roleID}",
                defaults: new { controller = "Roles", action = "RoleUsers" }
            );

            context.MapRoute(
                name: "LanguageBased_RoleUsers",
                url: "{lang}/dashboard/Roles/RoleUsers/{roleID}",
                defaults: new { controller = "Roles", action = "RoleUsers" }
            );

            context.MapRoute(
                name: "AssignUserRole",
                url: "dashboard/Roles/UserRole/{userID}/Assign/",
                defaults: new { controller = "Roles", action = "AssignUserRole" }
            );

            context.MapRoute(
                name: "LanguageBased_AssignUserRole",
                url: "{lang}/dashboard/Roles/UserRole/{userID}/Assign/",
                defaults: new { controller = "Roles", action = "AssignUserRole" }
            );

            context.MapRoute(
                name: "RemoveUserRole",
                url: "dashboard/Roles/UserRole/{userID}/Remove/",
                defaults: new { controller = "Roles", action = "RemoveUserRole" }
            );

            context.MapRoute(
                name: "LanguageBased_RemoveUserRole",
                url: "{lang}/dashboard/Roles/UserRole/{userID}/Remove/",
                defaults: new { controller = "Roles", action = "RemoveUserRole" }
            );

            context.MapRoute(
                name: "UserOrdersList",
                url: "dashboard/Users/Orders/",
                defaults: new { controller = "Orders", action = "UserOrders" }
            );

            context.MapRoute(
                name: "LanguageBased_UserOrdersList",
                url: "{lang}/dashboard/Users/Orders/",
                defaults: new { controller = "Orders", action = "UserOrders" }
            );

            context.MapRoute(
                name: "UpdateOrderStatus",
                url: "dashboard/Orders/{orderID}/Update-Status/",
                defaults: new { controller = "Orders", action = "UpdateStatus" }
            );

            context.MapRoute(
                name: "LanguageBased_UpdateOrderStatus",
                url: "{lang}/dashboard/Orders/{orderID}/Update-Status/",
                defaults: new { controller = "Orders", action = "UpdateStatus" }
            );

            context.MapRoute(
                "Dashboard_Default",
                "dashboard/{controller}/{action}/{id}",
                new { controller = "dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}