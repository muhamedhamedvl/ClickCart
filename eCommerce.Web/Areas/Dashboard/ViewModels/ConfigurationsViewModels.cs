using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class ConfigurationsListingViewModels : PageViewModel
    {
        public List<Configuration> Configurations { get; set; }

        public int? ConfigurationType { get; set; }
        public string SearchTerm { get; set; }

        public bool isPartial { get; set; }

        public Pager Pager { get; set; }
    }
}