using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class ProductsListingViewModel : PageViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
        public bool? ShowOnlyLowStock { get; set; }

        public Pager Pager { get; set; }
    }
    
    public class ProductActionViewModel : PageViewModel
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Cost { get; set; }
        public bool isFeatured { get; set; }
        public bool InActive { get; set; }

        public int StockQuantity { get; set; }

        public int ProductRecordID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public List<ProductSpecification> ProductSpecifications { get; set; }

        public string SKU { get; set; }
        public string Tags { get; set; }
        public string Barcode { get; set; }
        public string Supplier { get; set; }

        public string ProductPictures { get; set; }
        public int ThumbnailPicture { get; set; }
        public List<ProductPicture> ProductPicturesList { get; set; }

        public List<Category> Categories { get; set; }
    }
}