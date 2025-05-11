using eCommerce.Entities;
using eCommerce.Entities.APIEntities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.API.Models
{
    public class SearchProductsModel
    {
        public SearchProductsModel()
        {
            Products = new List<ProductEntity>();
            SearchFilters = new SearchFilters();
        }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public SearchFilters SearchFilters { get; set; }
        public List<ProductEntity> Products { get; set; }
    }

    public class SearchFilters
    {
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string SortBy { get; set; }
        public int PageNo { get; set; }
        public int? RecordSize { get; set; }
    }
}