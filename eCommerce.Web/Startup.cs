using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Helpers;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(eCommerce.Web.Startup))]
namespace eCommerce.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ConfigurationsHelper.UpdateConfigurations(ConfigurationsService.Instance.GetAllConfigurations());

            var enabledLanguages = LanguagesService.Instance.GetLanguages(enabledLanguagesOnly: true, resourceLanguagesOnly: false);
            
            if(enabledLanguages != null && enabledLanguages.Count > 0)
            {
                var languageIDsWithResources = LanguagesService.Instance.LanguagesWithResources();
                LanguagesHelper.LoadLanguages(enabledLanguages: enabledLanguages, defaultLanguage: enabledLanguages.FirstOrDefault(x => x.IsDefault), languageIDsWithResources: languageIDsWithResources);

                var resourcesForEnabledLanguages = LanguagesService.Instance.GetLanguagesResources(enabledLanguages.Select(x => x.ID).Distinct().ToList());
                LocalizationHelper.LoadResourceLocalizations(resourcesForEnabledLanguages);
            }
            else
            {
                throw new Exception("CriticalException: No Languages found.");
            }
        }
    }
}
