using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class PromosListingViewModel : PageViewModel
    {
        public List<Promo> Promos { get; set; }
        public string SearchTerm { get; set; }
        
        public Pager Pager { get; set; }
    }
    
    public class PromoActionViewModel : PageViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int PromoType { get; set; }
        public decimal Value { get; set; }
        public Nullable<DateTime> ValidTill { get; set; }
    }
}