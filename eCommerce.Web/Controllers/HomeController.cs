using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Controllers
{
    public class HomeController : PublicBaseController
    {
        public ActionResult Index()
        {
            /* if multi language is enabled and the home page is accessed without any language, redirect to the default language */
            if(ConfigurationsHelper.EnableMultilingual && Request.Url.AbsolutePath.ToString().Equals("/"))
            {
                return Redirect(Url.Home());
            }

            return View(new PageViewModel());
        }

        //[OutputCache(Duration = 600)]
        public ActionResult HomeSliders()
        {
            HomeSlidersViewModel model = new HomeSlidersViewModel
            {
                SlidersConfigurations = ConfigurationsService.Instance.GetConfigurationsByType((int)ConfigurationTypes.Sliders)
            };

            return PartialView("_BannerSlider", model);
        }

        public ActionResult Search(string category, string q, decimal? from, decimal? to, string sortby, int? pageNo, int? recordSize)
        {
            recordSize = recordSize ?? (int)RecordSizeEnums.Size20;

            ProductsViewModel model = new ProductsViewModel
            {
                Categories = CategoriesService.Instance.GetCategories()
            };

            if (!string.IsNullOrEmpty(category))
            {
                var selectedCategory = CategoriesService.Instance.GetCategoryByName(category);

                if (selectedCategory == null) return HttpNotFound();
                else
                {
                    model.CategoryID = selectedCategory.ID;
                    model.CategoryName = selectedCategory.SanitizedName;
                    model.SelectedCategory = selectedCategory;

                    model.SearchedCategories = CategoryHelpers.GetAllCategoryChildrens(selectedCategory, model.Categories);
                }
            }
            
            model.SearchTerm = q;
            model.PriceFrom = from;
            model.PriceTo = to;
            model.SortBy = sortby;
            model.PageSize = recordSize;
            
            var selectedCategoryIDs = model.SearchedCategories != null ? model.SearchedCategories.Select(x => x.ID).ToList() : null;

            model.Products = ProductsService.Instance.SearchProducts(selectedCategoryIDs, model.SearchTerm, model.PriceFrom, model.PriceTo, model.SortBy, pageNo, recordSize.Value, activeOnly: true, out int count, stockCheckCount: null);

            model.Pager = new Pager(count, pageNo, recordSize.Value);

            return View(model);
        }

        public ActionResult PriceRangeFilter(decimal? priceFrom, decimal? priceTo)
        {
            var model = new PriceRangeFilterViewModel
            {
                PriceFrom = priceFrom,
                PriceTo = priceTo,

                MaxPrice = ProductsService.Instance.GetMaxProductPrice()
            };

            return PartialView("SearchFilters/_PriceRangeFilter", model);
        }

        [ValidateAntiForgeryToken]
        public JsonResult SubscribeNewsLetter(string email)
        {
            JsonResult jsonResult = new JsonResult();

            var newsletterSubscription = new NewsletterSubscription()
            {
                EmailAddress = email,
                IsVerified = false,
                UserID = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : string.Empty,
                ModifiedOn = DateTime.Now,
                IsActive = true
            };

            var result = SharedService.Instance.SaveNewsletterSubscription(newsletterSubscription);

            if(result)
            {
                jsonResult.Data = new
                {
                    Success = true,
                    Message = "PP.Footer.NewsLetter.NewsletterSubscribed".LocalizedString()
                };
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = "PP.Footer.NewsLetter.NewsletterAlreadySubscribed".LocalizedString()
                };
            }

            return jsonResult;
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SubmitContactForm(string subject, string name, string email, string message)
        {
            JsonResult jsonResult = new JsonResult();

            try
            {
                //send order placed notification email to admin emails
                await new EmailService()
                                 .SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName,
                                                   ConfigurationsHelper.SendGrid_FromEmailAddress,
                                                   ConfigurationsHelper.AdminEmailAddress,
                                                   EmailTextHelpers.ContactMessageSubject_Admin(),
                                                   EmailTextHelpers.ContactMessageBody_Admin(subject, name, email, message));

                jsonResult.Data = new
                {
                    Success = true,
                    Message = "Your message has been submitted. We will contact you back soon."
                };
            }
            catch (Exception)
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = "An error occured while submitting your message."
                };
            }

            return jsonResult;
        }
    }
}