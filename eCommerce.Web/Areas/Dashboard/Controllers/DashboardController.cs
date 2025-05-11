using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Enums;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class DashboardController : DashboardBaseController
    {
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            CategoriesService.Instance.SearchCategories(parentCategoryID: null, searchTerm: null, pageNo: null, recordSize: 0, count: out int categoriesCount);
            model.CategoriesCount = categoriesCount;

            ProductsService.Instance.SearchProducts(categoryIDs: null, searchTerm: null, from: null, to: null, sortby: null, pageNo: null, recordSize: 0, activeOnly: true, stockCheckCount: null, count: out int productsCount);
            model.ProductsCount = productsCount;

            OrdersService.Instance.SearchOrders(userID: null, orderID: null, orderStatus: null, pageNo: null, recordSize: 0, count: out int ordersCount);
            model.OrdersCount = ordersCount;

            CommentsService.Instance.SearchComments(entityID: (int)EntityEnums.Product, recordID: null, userID: null, searchTerm: null, pageNo: null, recordSize: 0, count: out int commentsCount);
            model.CommentsCount = commentsCount;

            model.UserCount = DashboardService.Instance.GetUserCount();
            model.RolesCount = DashboardService.Instance.GetRolesCount();

            model.ProductsWithLessStockQuantity = ProductsService.Instance.GetProductWithLessStockQuantity(categoryIDs: null, searchTerm: null, from: null, to: null, sortby: null, activeOnly: true, stockCount: 5, count: out int lessProductsCount);

            return View(model);
        }

        public ActionResult NewsletterSubscribers(string searchTerm, int? pageNo)
        {
            var recordSize = (int)RecordSizeEnums.Size10;

            NewsletterSubscribersListingViewModel model = new NewsletterSubscribersListingViewModel
            {
                SearchTerm = searchTerm
            };

            model.NewsletterSubscribers = SharedService.Instance.SearchNewsletterSubscription(searchTerm, pageNo, recordSize, out int count);

            model.Pager = new Pager(count, pageNo, recordSize);

            return View(model);
        }
    }
}