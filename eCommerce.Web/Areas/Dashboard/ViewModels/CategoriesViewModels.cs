using eCommerce.Entities;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.Dashboard.ViewModels
{
    public class CategoriesListingViewModel : PageViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Category> ParentCategories { get; set; }

        public int? ParentCategoryID { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }
    
    public class CategoryActionViewModel : PageViewModel
    {
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public int? ParentCategoryID { get; set; }
        public bool isFeatured { get; set; }
        public string SanitizedName { get; set; }
        public int DisplaySeqNo { get; set; }


        public int? CategoryRecordID { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public int? PictureID { get; set; }
        public virtual Picture Picture { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual List<CategoryRecord> CategoryRecords { get; set; }
        public List<Category> Categories { get; set; }
    }
}