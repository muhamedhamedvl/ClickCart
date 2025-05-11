using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.ViewModels
{
    public class CategoriesMenuViewModel
    {
        public List<CategoryWithChildren> CategoryWithChildrens { get; set; }
    }
}