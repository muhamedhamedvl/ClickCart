using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.ViewModels
{
    public class ProductsViewModel : PageViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Category SelectedCategory { get; set; }
        public List<Category> SearchedCategories { get; set; }

        public string SearchTerm { get; set; }
        public string SortBy { get; set; }

        public Pager Pager { get; set; }
        public int PageNo { get; set; }
        public int? PageSize { get; set; }

        public List<string> Brands { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }

    public class FeaturedProductsViewModel : PageViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
    public class RelatedProductsViewModel : FeaturedProductsViewModel
    {
        public Category Category { get; set; }
        public bool IsFeaturedProductsOnly { get; set; }
    }

    public class ProductsByFeaturedCategoriesViewModel : PageViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }

    public class FeaturedCategoriesAndProductsSectionsViewModel : PageViewModel
    {
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> Products { get; set; }
    }
    public class CartProductsViewModel
    {
        public List<int> ProductIDs { get; set; }
        public List<Product> Products { get; set; }
    }

    public class ProductDetailsViewModel : CommentablePageViewModel
    {
        public Product Product { get; set; }
        public ProductRating Rating { get; internal set; }
    }
}