using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.ViewModels
{
    public class PriceRangeFilterViewModel
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public decimal MaxPrice { get; set; }
    }
}