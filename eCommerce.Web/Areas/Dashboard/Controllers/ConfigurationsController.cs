using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class ConfigurationsController : DashboardBaseController
    {
        public ActionResult Index(int? configurationType, string searchTerm, int? pageNo, bool isPartial = false)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            ConfigurationsListingViewModels model = new ConfigurationsListingViewModels
            {
                ConfigurationType = configurationType,
                SearchTerm = searchTerm,

                Configurations = ConfigurationsService.Instance.SearchConfigurations(configurationType, searchTerm, pageNo, pageSize, out int configurationsCount),

                Pager = new Pager(configurationsCount, pageNo, pageSize)
            };

            if (isPartial)
            {
                return PartialView("_Listing", model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Action(string key)
        {
            var configuration = ConfigurationsService.Instance.GetConfigurationByKey(key);

            if (configuration == null) return HttpNotFound();

            if (configuration.ConfigurationType == (int)ConfigurationTypes.Sliders)
            {
                return PartialView("_HomeSlidersEdit", configuration);
            }
            else return PartialView("_Edit", configuration);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Action(Configuration configuration)
        {
            var json = new JsonResult();

            try
            {
                if (configuration == null)
                {
                    throw new Exception("Dashboard.Configurations.ConfigurationNotFound".LocalizedString());
                }

                var result = ConfigurationsService.Instance.UpdateConfigurationValue(configuration.Key, configuration.Value);

                if(result)
                {
                    json.Data = new { Success = true };
                }
                else
                {
                    throw new Exception("Dashboard.Configurations.UnableToUpdateConfigurations".LocalizedString());
                }

                ConfigurationsHelper.UpdateConfiguration(configuration);
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }
    }
}