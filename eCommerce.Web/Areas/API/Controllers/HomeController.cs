using eCommerce.Entities;
using eCommerce.Entities.APIEntities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Services;
using eCommerce.Shared.Commons;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.Areas.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eCommerce.Web.Areas.API.Controllers
{
    public class HomeController : APIBaseController
    {
        [HttpGet]
        public IHttpActionResult StoreInfo()
        {
            var model = new StoreInfoModel()
            {
                StoreName = ConfigurationsHelper.ApplicationName,
                StoreIntro = ConfigurationsHelper.ApplicationIntro,
                PhoneNumber = ConfigurationsHelper.PhoneNumber,
                MobileNumber = ConfigurationsHelper.MobileNumber,
                WhatsAppNumber = ConfigurationsHelper.WhatsAppNumber,
                Email = ConfigurationsHelper.Email,
                FacebookURL = ConfigurationsHelper.FacebookURL,
                TwitterURL = ConfigurationsHelper.TwitterURL,
                InstagramURL = ConfigurationsHelper.InstagramURL,
                YouTubeURL = ConfigurationsHelper.YouTubeURL,
                LinkedInURL = ConfigurationsHelper.LinkedInURL,
                Address = string.Format("{0} {1}", ConfigurationsHelper.AddressLine1, ConfigurationsHelper.AddressLine2),
                CurrencySymbol = ConfigurationsHelper.CurrencySymbol
            };

            return APIResult(HttpStatusCode.OK, model);
        }

        [HttpGet]
        public IHttpActionResult Sliders()
        {
            var sliders = new List<Slider>();

            var SlidersConfigurations = ConfigurationsService.Instance.GetConfigurationsByType((int)ConfigurationTypes.Sliders);
            if(SlidersConfigurations != null && SlidersConfigurations.Count > 0)
            {
                foreach (var sliderConfig in SlidersConfigurations)
                {
                    var img = sliderConfig.Value.GetSubstringText("#IMG#", "#TH#");
                    var topHeading = sliderConfig.Value.GetSubstringText("#TH#", "#PG#");
                    var paragraph = sliderConfig.Value.GetSubstringText("#PG#", "#LK#");
                    var link = sliderConfig.Value.GetSubstringText("#LK#", "");

                    if (!string.IsNullOrEmpty(img))
                    {
                        img = img.Replace("IMG#", "");
                    }
                    if (!string.IsNullOrEmpty(topHeading))
                    {
                        topHeading = topHeading.Replace("TH#", "");
                    }
                    if (!string.IsNullOrEmpty(paragraph))
                    {
                        paragraph = paragraph.Replace("PG#", "");
                    }
                    if (!string.IsNullOrEmpty(link))
                    {
                        link = link.Replace("LK#", "");
                    }

                    sliders.Add(new Slider() {
                        ImageLink = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, img),
                        Heading = topHeading,
                        Summary = paragraph,
                        Link = link
                    });
                }
            }

            return APIResult(HttpStatusCode.OK, sliders);
        }

        [HttpGet]
        public IHttpActionResult FeaturedCategories(int? recordSize = 3)
        {
            var categories = CategoriesService.Instance.GetFeaturedCategories(recordSize: recordSize);

            var featuredCategories = new List<CategoryEntity>();
            if (categories != null && categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    var currentLanguageCategoryRecord = category.CategoryRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    featuredCategories.Add(new CategoryEntity()
                    {
                        ID = category.ID,
                        Name = currentLanguageCategoryRecord.Name,
                        Summary = currentLanguageCategoryRecord.Summary,
                        Description = currentLanguageCategoryRecord.Description,
                        IsFeatured = category.isFeatured,
                        SanitizedName = category.SanitizedName,
                        Picture = category.Picture != null ? string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, category.Picture.URL) : string.Empty,
                        ProductsCount = category.Products != null ? category.Products.Count : 0
                    });
                }
            }

            return APIResult(HttpStatusCode.OK, featuredCategories);
        }

        [HttpGet]
        public IHttpActionResult CategoriesMenu(int? recordSize = 3)
        {
            var categories = CategoriesService.Instance.GetCategories();

            var menuCategories = new List<MenuCategory>();
            if (categories != null && categories.Count > 0)
            {
                //remove uncategorized category from categories list.
                categories = categories.Where(x => x.ID != 1).ToList();

                //for now I'm assuming we will only have one sub level categories
                foreach (var topCategory in categories.Where(x=>!x.ParentCategoryID.HasValue))
                {
                    var childrenMenu = new List<MenuCategory>();

                    var childCategories = categories.Where(x => x.ParentCategoryID.HasValue && x.ParentCategoryID.Value == topCategory.ID).ToList();
                    if(childCategories != null && childCategories.Count > 0)
                    {
                        foreach (var childCategory in childCategories)
                        {
                            var currentLanguageChildCategoryRecord = childCategory.CategoryRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                            childrenMenu.Add(new MenuCategory()
                            {
                                ID = childCategory.ID,
                                Name = currentLanguageChildCategoryRecord.Name,
                                SanitizedName = childCategory.SanitizedName,
                                Picture = childCategory.Picture != null ? string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, childCategory.Picture.URL) : string.Empty,
                                ProductsCount = childCategory.Products != null ? topCategory.Products.Count : 0
                            });
                        }
                    }

                    var currentLanguageCategoryRecord = topCategory.CategoryRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    menuCategories.Add(new MenuCategory()
                    {
                        ID = topCategory.ID,
                        Name = currentLanguageCategoryRecord.Name,
                        SanitizedName = topCategory.SanitizedName,
                        Picture = topCategory.Picture != null ? string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, topCategory.Picture.URL) : string.Empty,
                        ProductsCount = topCategory.Products != null ? topCategory.Products.Count : 0,
                        Children = childrenMenu
                    });
                }
            }

            return APIResult(HttpStatusCode.OK, menuCategories);
        }

        [HttpGet]
        public IHttpActionResult Configurations(string key = "")
        {
            if(!string.IsNullOrEmpty(key))
            {
                if(ConfigurationsHelper.ConfigurationsDictionary.ContainsKey(key))
                {
                    return APIResult(HttpStatusCode.OK, new { Key = key, Value = ConfigurationsHelper.ConfigurationsDictionary[key] });
                }
                else return APIResult(HttpStatusCode.NotFound);
            }
            else return APIResult(HttpStatusCode.OK, ConfigurationsHelper.ConfigurationsDictionary.OrderBy(x=>x.Key));
        }

        [HttpGet]
        public IHttpActionResult FeaturedProducts(int? recordSize = 3)
        {
            var featuredProducts = ProductsService.Instance.SearchFeaturedProducts(recordSize: recordSize);

            if(featuredProducts != null && featuredProducts.Count > 0)
            {
                var productsList = new List<ProductEntity>();

                foreach (var product in featuredProducts)
                {
                    var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    productsList.Add(new ProductEntity()
                    {
                        ID = product.ID,
                        CategoryID = product.CategoryID,
                        Name = currentLanguageProductRecord.Name,
                        Summary = currentLanguageProductRecord.Summary,
                        Description = currentLanguageProductRecord.Description,
                        Price = product.Price,
                        Discount = product.Discount,
                        Cost = product.Cost,
                        IsFeatured = product.isFeatured,
                        SKU = product.SKU,
                        Tags = product.Tags,
                        Barcode = product.Barcode,
                        Supplier = product.Supplier,
                        StockQuantity = product.StockQuantity,
                        ProductSpecifications = currentLanguageProductRecord.ProductSpecifications != null ? currentLanguageProductRecord.ProductSpecifications.Select(x => new ProductSpecificationEntity() { Title = x.Title, Value = x.Value }).ToList() : new List<ProductSpecificationEntity>(),
                        Pictures = product.ProductPictures != null ? product.ProductPictures.Where(x => x.Picture != null).Select(x => new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, x.Picture.URL), IsThumbnail = product.ThumbnailPictureID == x.PictureID }).ToList() : new List<PictureEntity>() { new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.DEFAULT_PICTURE), IsThumbnail = true } }
                    });
                }
                
                return APIResult(HttpStatusCode.OK, productsList);
            }
            else return APIResult(HttpStatusCode.OK, featuredProducts);
        }

        [HttpGet]
        public IHttpActionResult Search(string category = "", int? categoryID = 0, string q = "", decimal? priceFrom = 0, decimal? priceTo = 0, string sortby = "", int? pageNo = 1, int? recordSize = (int)RecordSizeEnums.Size20)
        {
            var model = new SearchProductsModel();

            var SearchedCategories = new List<Category>();
            if (!string.IsNullOrEmpty(category) || (categoryID.HasValue && categoryID.Value > 0))
            {
                var selectedCategory = !string.IsNullOrEmpty(category) ? CategoriesService.Instance.GetCategoryByName(category) :
                                       CategoriesService.Instance.GetCategoryByID(categoryID.Value);

                if (selectedCategory == null)
                {
                    return APIResult(HttpStatusCode.NotFound);
                }
                else
                {
                    var allCategories = CategoriesService.Instance.GetCategories();

                    model.SearchFilters.CategoryID = selectedCategory.ID;

                    SearchedCategories = CategoryHelpers.GetAllCategoryChildrens(selectedCategory, allCategories);
                }
            }

            model.SearchFilters.SearchTerm = q;
            model.SearchFilters.PriceFrom = priceFrom;
            model.SearchFilters.PriceTo = priceTo;
            model.SearchFilters.SortBy = sortby;
            model.SearchFilters.PageNo = pageNo.Value;
            model.SearchFilters.RecordSize = recordSize;

            var selectedCategoryIDs = SearchedCategories != null ? SearchedCategories.Select(x => x.ID).ToList() : null;

            var products = ProductsService.Instance.SearchProducts(selectedCategoryIDs, model.SearchFilters.SearchTerm, model.SearchFilters.PriceFrom, model.SearchFilters.PriceTo, model.SearchFilters.SortBy, model.SearchFilters.PageNo, model.SearchFilters.RecordSize.Value, activeOnly: true, out int count);

            if(products != null && products.Count > 0)
            {
                foreach (var product in products)
                {
                    var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    model.Products.Add(new ProductEntity()
                    {
                        ID = product.ID,
                        CategoryID = product.CategoryID,
                        Name = currentLanguageProductRecord.Name,
                        Summary = currentLanguageProductRecord.Summary,
                        Description = currentLanguageProductRecord.Description,
                        Price = product.Price,
                        Discount = product.Discount,
                        Cost = product.Cost,
                        IsFeatured = product.isFeatured,
                        SKU = product.SKU,
                        Tags = product.Tags,
                        Barcode = product.Barcode,
                        Supplier = product.Supplier,
                        StockQuantity = product.StockQuantity,
                        ProductSpecifications = currentLanguageProductRecord.ProductSpecifications != null ? currentLanguageProductRecord.ProductSpecifications.Select(x => new ProductSpecificationEntity() { Title = x.Title, Value = x.Value }).ToList() : new List<ProductSpecificationEntity>(),
                        Pictures = product.ProductPictures != null ? product.ProductPictures.Where(x=>x.Picture != null).Select(x=> new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, x.Picture.URL), IsThumbnail = product.ThumbnailPictureID == x.PictureID }).ToList() : new List<PictureEntity>() { new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.DEFAULT_PICTURE), IsThumbnail = true } }
                    });
                }
            }

            model.TotalRecords = count;

            return APIResult(HttpStatusCode.OK, model);
        }

        [HttpGet]
        public IHttpActionResult ProductDetails(int ID)
        {
            var product = ProductsService.Instance.GetProductByID(ID);

            if (product != null)
            {
                var rating = CommentsService.Instance.GetProductRating(product.ID);

                var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                return APIResult(HttpStatusCode.OK, new ProductEntity()
                {
                    ID = product.ID,
                    CategoryID = product.CategoryID,
                    Name = currentLanguageProductRecord.Name,
                    Summary = currentLanguageProductRecord.Summary,
                    Description = currentLanguageProductRecord.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    Cost = product.Cost,
                    IsFeatured = product.isFeatured,
                    SKU = product.SKU,
                    Tags = product.Tags,
                    Barcode = product.Barcode,
                    Supplier = product.Supplier,
                    StockQuantity = product.StockQuantity,
                    TotalRatings = rating.TotalRatings,
                    AverageRatings = rating.AverageRating,
                    ProductSpecifications = currentLanguageProductRecord.ProductSpecifications != null ? currentLanguageProductRecord.ProductSpecifications.Select(x => new ProductSpecificationEntity() { Title = x.Title, Value = x.Value }).ToList() : new List<ProductSpecificationEntity>(),
                    Pictures = product.ProductPictures != null ? product.ProductPictures.Where(x => x.Picture != null).Select(x => new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, x.Picture.URL), IsThumbnail = product.ThumbnailPictureID == x.PictureID }).ToList() : new List<PictureEntity>() { new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.DEFAULT_PICTURE), IsThumbnail = true } }
                });
            }
            else
            {
                return APIResult(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public IHttpActionResult ProductComments(int ID, int pageNo = 1, int recordSize = (int)RecordSizeEnums.Size10)
        {
            recordSize = recordSize > 0 ? recordSize : (int)RecordSizeEnums.Size10;

            var product = ProductsService.Instance.GetProductByID(ID);

            if(product == null)
            {
                return APIResult(HttpStatusCode.NotFound);
            }

            var model = new CommentsListModel();
            model.CommentsListFilters.PageNo = pageNo;
            model.CommentsListFilters.RecordSize = recordSize;

            var comments = CommentsService.Instance.SearchComments(entityID: (int)EntityEnums.Product, recordID: ID, userID: null, searchTerm: null, pageNo: pageNo, recordSize: recordSize, out int count);
            
            if(comments!= null && comments.Count > 0)
            {
                model.TotalComments = count;
                model.TotalPages = count / recordSize;

                var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                comments.ForEach(x => model.Comments.Add(new ProductCommentEntity()
                {
                    ID = x.ID,
                    Product = new CommentProduct() {
                        ProductID = product.ID,
                        ProductTitle = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Empty
                    },
                    TimeStamp = x.TimeStamp,
                    UserID = x.UserID, 
                    UserName = x.User != null ? x.User.UserName : string.Empty,
                    Text = x.Text,
                    Rating = x.Rating
                }));
            }

            return APIResult(HttpStatusCode.OK, model);
        }
        
        [HttpGet]
        public IHttpActionResult RelatedProducts(int ID, int recordSize = (int)RecordSizeEnums.Size6)
        {
            var productList = new List<ProductEntity>();
         
            var product = ProductsService.Instance.GetProductByID(ID);

            var productCategoryID = product != null ? product.CategoryID : 0;

            var relatedProducts = new List<Product>();

            if(productCategoryID > 0)
            {
                relatedProducts = ProductsService.Instance.SearchProducts(new List<int>() { productCategoryID }, null, null, null, null, 1, recordSize, activeOnly: true, out int count);
            }

            if (relatedProducts.Count < (int)RecordSizeEnums.Size6)
            {
                //the realted products are less than the specfified RelatedProductsRecordsSize, so instead show featured products
                relatedProducts = ProductsService.Instance.SearchFeaturedProducts(recordSize);
            }

            if(relatedProducts != null && relatedProducts.Count > 0)
            {
                foreach (var relatedProduct in relatedProducts)
                {
                    var currentLanguageProductRecord = relatedProduct.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                    productList.Add(new ProductEntity()
                    {
                        ID = relatedProduct.ID,
                        CategoryID = relatedProduct.CategoryID,
                        Name = currentLanguageProductRecord.Name,
                        Summary = currentLanguageProductRecord.Summary,
                        Description = currentLanguageProductRecord.Description,
                        Price = relatedProduct.Price,
                        Discount = relatedProduct.Discount,
                        Cost = relatedProduct.Cost,
                        IsFeatured = relatedProduct.isFeatured,
                        SKU = relatedProduct.SKU,
                        Tags = relatedProduct.Tags,
                        Barcode = relatedProduct.Barcode,
                        Supplier = relatedProduct.Supplier,
                        StockQuantity = relatedProduct.StockQuantity,
                        ProductSpecifications = currentLanguageProductRecord.ProductSpecifications != null ? currentLanguageProductRecord.ProductSpecifications.Select(x => new ProductSpecificationEntity() { Title = x.Title, Value = x.Value }).ToList() : new List<ProductSpecificationEntity>(),
                        Pictures = relatedProduct.ProductPictures != null ? relatedProduct.ProductPictures.Where(x => x.Picture != null).Select(x => new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, x.Picture.URL), IsThumbnail = relatedProduct.ThumbnailPictureID == x.PictureID }).ToList() : new List<PictureEntity>() { new PictureEntity() { URL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.DEFAULT_PICTURE), IsThumbnail = true } }
                    });
                }
            }

            return APIResult(HttpStatusCode.OK, productList);
        }
    }
}
