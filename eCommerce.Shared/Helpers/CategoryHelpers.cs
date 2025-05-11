using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Shared.Helpers
{
    public class CategoryHelpers
    {
        public static List<CategoryWithChildren> MakeCategoriesHierarchy(List<Category> allCategories)
        {
            if (allCategories != null && allCategories.Count > 0)
            {
                var categoryWithChildren = allCategories.Select(item => new CategoryWithChildren() { Category = item, Children = allCategories.Where(child => child.ParentCategoryID == item.ID).OrderBy(x=>x.SanitizedName).ToList() }).ToList();

                return categoryWithChildren.Where(item => !item.Category.ParentCategoryID.HasValue).ToList();
            }

            return null;
        }

        public static List<Category> GetCategoryHierarchy(Category category, List<Category> allCategories)
        {
            if (category != null && allCategories != null && allCategories.Count > 0)
            {
                var categories = new List<Category>() { category };

                Category parentCategory = null;

                var parentCategoryID = category.ParentCategoryID;

                do
                {
                    parentCategory = GetCategoryParent(parentCategoryID, allCategories);

                    if (parentCategory != null)
                    {
                        categories.Add(parentCategory);

                        parentCategoryID = parentCategory.ParentCategoryID;
                    }
                    else
                    {
                        parentCategoryID = null;
                    }
                } while (parentCategory != null);

                categories.Reverse();

                return categories;
            }

            return null;
        }

        public static Category GetCategoryParent(int? parentCategoryID, List<Category> allCategories)
        {
            return parentCategoryID.HasValue ? allCategories.FirstOrDefault(x => x.ID == parentCategoryID) : null;
        }

        public static List<Category> GetAllCategoryChildrens(Category category, List<Category> allCategories)
        {
            if (category != null && allCategories != null && allCategories.Count > 0)
            {
                var categories = new List<Category>() { category };

                var childCategories = GetCategoryChildren(category.ID, allCategories);

                foreach (var childCategory in childCategories)
                {
                    categories.Add(childCategory);

                    GetAllCategoryChildrens(childCategory, allCategories);
                }

                return categories;
            }

            return null;
        }

        public static List<Category> GetCategoryChildren(int parentCategoryID, List<Category> allCategories)
        {
            return allCategories.Where(x => x.ParentCategoryID == parentCategoryID).ToList();
        }

        public static int GetCategoryAllChildrensProductCount(Category category, List<Category> allCategories)
        {
            if (category != null && allCategories != null && allCategories.Count > 0)
            {
                int productCount = category.Products != null ? category.Products.Where(x=>!x.IsDeleted).Count() : 0;

                var childCategories = GetCategoryChildren(category.ID, allCategories);

                foreach (var childCategory in childCategories)
                {
                    productCount += GetCategoryAllChildrensProductCount(childCategory, allCategories);
                }

                return productCount;
            }

            return 0;
        }
    }
}