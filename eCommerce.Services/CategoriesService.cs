using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class CategoriesService
    {
        #region Define as Singleton
        private static CategoriesService _Instance;

        public static CategoriesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CategoriesService();
                }

                return (_Instance);
            }
        }

        private CategoriesService()
        {
        }
        #endregion

        public List<Category> GetCategories(int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var categories = context.Categories
                                    .Where(x=>!x.IsDeleted)
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();
            
            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                categories = categories.Skip(skip)
                                       .Take(recordSize.Value);
            }

            return categories.ToList();
        }

        public List<Category> GetFeaturedCategories(int? pageNo = 1, int? recordSize = 0, bool includeProducts = false)
        {
            var context = DataContextHelper.GetNewContext();

            var categories = context.Categories
                                    .Where(x => !x.IsDeleted && x.isFeatured)
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                categories = categories.Skip(skip)
                                       .Take(recordSize.Value);
            }

            if(includeProducts)
            {
                categories = categories.Include("Products.ProductRecords");
            }

            return categories.ToList();
        }

        public List<CategoryRecord> GetCategoriesRecordsByCategory(int categoryID, int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var categoryRecords = context.CategoryRecords
                                         .Where(x=>x.CategoryID == categoryID && !x.IsDeleted)
                                         .OrderBy(x => x.ID)
                                         .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                categoryRecords = categoryRecords.Skip(skip)
                                                 .Take(recordSize.Value);
            }

            return categoryRecords.ToList();
        }

        public List<CategoryRecord> GetCategoriesRecordsByLanguage(int languageID, int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var categoryRecords = context.CategoryRecords
                                         .Where(x => x.LanguageID == languageID && !x.IsDeleted)
                                         .OrderBy(x => x.ID)
                                         .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                categoryRecords = categoryRecords.Skip(skip)
                                                 .Take(recordSize.Value);
            }

            return categoryRecords.ToList();
        }

        public List<Category> GetAllTopLevelCategories(int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var categories = context.Categories
                                    .Where(x => !x.ParentCategoryID.HasValue && !x.IsDeleted)
                                    .OrderBy(x => x.ID)
                                    .AsQueryable();

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                categories = categories.Skip(skip)
                                       .Take(recordSize.Value);
            }

            return categories.ToList();
        }

        public Category GetCategoryByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var category = context.Categories.Find(ID);

            return category != null && !category.IsDeleted ? category : null;
        }

        public CategoryRecord GetCategoryRecordByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var categoryRecord = context.CategoryRecords.Find(ID);

            return categoryRecord != null && !categoryRecord.IsDeleted ? categoryRecord : null;
        }

        public Category GetCategoryByName(string sanitizedCategoryName)
        {
            var context = DataContextHelper.GetNewContext();

            var category = context.Categories.FirstOrDefault(x => x.SanitizedName.Equals(sanitizedCategoryName) && !x.IsDeleted);

            return category != null ? category : null;
        }

        public bool SaveCategory(Category category)
        {
            var context = DataContextHelper.GetNewContext();

            context.Categories.Add(category);

            return context.SaveChanges() > 0;
        }

        public bool SaveCategoryRecord(CategoryRecord categoryRecord)
        {
            var context = DataContextHelper.GetNewContext();

            context.CategoryRecords.Add(categoryRecord);

            return context.SaveChanges() > 0;
        }

        public bool UpdateCategory(Category category)
        {
            var context = DataContextHelper.GetNewContext();

            var existingCategory = context.Categories.Find(category.ID);

            context.Entry(existingCategory).CurrentValues.SetValues(category);
            
            return context.SaveChanges() > 0;
        }
        
        public bool UpdateCategoryRecord(CategoryRecord categoryRecord)
        {
            var context = DataContextHelper.GetNewContext();

            var existingRecord = context.CategoryRecords.Find(categoryRecord.ID);

            context.Entry(existingRecord).CurrentValues.SetValues(categoryRecord);

            return context.SaveChanges() > 0;
        }

        public bool DeleteCategory(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var category = context.Categories.Find(ID);

            category.IsDeleted = true;

            context.Entry(category).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public List<Category> SearchCategories(int? parentCategoryID, string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var categories = context.Categories.Where(x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                categories = context.CategoryRecords
                                    .Where(x => !x.Category.IsDeleted && x.Name.ToLower().Contains(searchTerm.ToLower()))
                                    .Select(x => x.Category)
                                    .AsQueryable();
            }

            if (parentCategoryID.HasValue && parentCategoryID.Value > 0)
            {
                categories = categories.Where(x => x.ParentCategoryID == parentCategoryID.Value);
            }

            count = categories.Count();
            
            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return categories.OrderBy(x => x.ID).Skip(skipCount).Take(recordSize).ToList();
        }
    }
}
