using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class LanguagesListingViewModel : PageViewModel
    {
        public List<Language> Languages { get; set; }
        public List<int> LanguageIDsWithResources { get; set; }

        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }
    
    public class LanguageActionViewModel : PageViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public bool IsEnabled { get; set; }
        public string ResourceFileName { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDefault { get; set; }

        public string IconCode { get; set; }
    }

    public class LanguageResourceViewModel : PageViewModel
    {
        public int ID { get; set; }
        public Language Language { get; set; }
        public bool HasResources { get; set; }
    }
}