using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class CategoriesController : DashboardBaseController
    {
        public ActionResult Index(int? parentCategoryID, string searchTerm, int? pageNo)
        {
            var recordSize = (int)RecordSizeEnums.Size10;

            CategoriesListingViewModel model = new CategoriesListingViewModel
            {
                ParentCategoryID = parentCategoryID,
                SearchTerm = searchTerm,

                ParentCategories = CategoriesService.Instance.GetAllTopLevelCategories()
            };

            model.Categories = CategoriesService.Instance.SearchCategories(parentCategoryID, searchTerm, pageNo, recordSize, out int count);

            model.Pager = new Pager(count, pageNo, recordSize);

            return View(model);
        }
                
        [HttpGet]
        public ActionResult Action(int? ID)
        {
            CategoryActionViewModel model = new CategoryActionViewModel();
            
            if (ID.HasValue)
            {
                var category = CategoriesService.Instance.GetCategoryByID(ID.Value);

                if (category == null) return HttpNotFound();

                var currentLanguageRecord = category.CategoryRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                currentLanguageRecord = currentLanguageRecord ?? new CategoryRecord();

                model.CategoryID = category.ID;
                model.ParentCategoryID = category.ParentCategoryID.HasValue ? category.ParentCategoryID.Value : 0;
                model.isFeatured = category.isFeatured;
                model.SanitizedName = category.SanitizedName;
                model.Picture = category.Picture;
                model.PictureID = category.PictureID;

                model.CategoryRecordID = currentLanguageRecord.ID;
                model.Name = currentLanguageRecord.Name;
                model.Description = currentLanguageRecord.Description;
                model.Summary = currentLanguageRecord.Summary;                
            }

            model.Categories = CategoriesService.Instance.GetCategories();

            return View(model);
        }

        [HttpPost]
        public JsonResult Action(CategoryActionViewModel model)
        {
            JsonResult json = new JsonResult();

            try
            {
                if (model.CategoryID > 0)
                {
                    var category = CategoriesService.Instance.GetCategoryByID(model.CategoryID);

                    if (category == null)
                    {
                        throw new Exception("Dashboard.Categories.Action.Validation.CategoryNotFound".LocalizedString());
                    }

                    if (model.ParentCategoryID > 0)
                    {
                        category.ParentCategoryID = model.ParentCategoryID;
                    }
                    else
                    {
                        category.ParentCategoryID = null;
                        category.ParentCategory = null;
                    }

                    category.PictureID = model.PictureID;
                    category.isFeatured = model.isFeatured;
                    category.SanitizedName = !string.IsNullOrEmpty(model.SanitizedName) ? model.SanitizedName : model.Name.SanitizeLowerString();

                    category.ModifiedOn = DateTime.Now;

                    if(!CategoriesService.Instance.UpdateCategory(category))
                    {
                        throw new Exception("Dashboard.Categories.Action.Validation.UnableToUpdateCategory".LocalizedString());
                    }

                    var currentCategoryRecord = category.CategoryRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    var isNewRecord = false;

                    if (currentCategoryRecord == null)
                    {
                        currentCategoryRecord = new CategoryRecord();
                        isNewRecord = true;
                    }

                    currentCategoryRecord.CategoryID = category.ID;
                    currentCategoryRecord.LanguageID = AppDataHelper.CurrentLanguage.ID;
                    currentCategoryRecord.Name = model.Name;
                    currentCategoryRecord.Description = model.Description;
                    currentCategoryRecord.Summary = model.Summary;

                    currentCategoryRecord.ModifiedOn = DateTime.Now;

                    var result = false;
                    if (isNewRecord)
                    {
                        result = CategoriesService.Instance.SaveCategoryRecord(currentCategoryRecord);
                    }
                    else
                    {
                        result = CategoriesService.Instance.UpdateCategoryRecord(currentCategoryRecord);
                    }

                    if (!result)
                    {
                        throw new Exception("Dashboard.Categories.Action.Validation.UnableToUpdateCategoryRecord".LocalizedString());
                    }
                }
                else
                {
                    var category = new Category();

                    if (model.ParentCategoryID > 0)
                    {
                        category.ParentCategoryID = model.ParentCategoryID;
                    }
                    else
                    {
                        category.ParentCategoryID = null;
                        category.ParentCategory = null;
                    }

                    category.PictureID = model.PictureID;
                    category.isFeatured = model.isFeatured;
                    category.SanitizedName = !string.IsNullOrEmpty(model.SanitizedName) ? model.SanitizedName : model.Name.SanitizeLowerString();

                    var currentLanguageCategoryRecord = new CategoryRecord
                    {
                        Category = category,
                        LanguageID = AppDataHelper.CurrentLanguage.ID,
                        Name = model.Name,
                        Description = model.Description,
                        Summary = model.Summary,
                        ModifiedOn = DateTime.Now
                    };

                    var result = CategoriesService.Instance.SaveCategoryRecord(currentLanguageCategoryRecord);

                    if (!result)
                    {
                        throw new Exception("Dashboard.Categories.Action.Validation.UnableToCreateCategory".LocalizedString());
                    }
                }

                json.Data = new { Success = true };
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var operation = CategoriesService.Instance.DeleteCategory(ID);

                result.Data = new { Success = operation, Message = operation ? string.Empty : "Dashboard.Categories.Action.Validation.UnableToDeleteCategory".LocalizedString() };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
            }
            
            return result;
        }
    }
}