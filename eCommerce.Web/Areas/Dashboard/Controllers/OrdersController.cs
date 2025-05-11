using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class OrdersController : DashboardBaseController
    {
        private eCommerceUserManager _userManager;

        public OrdersController()
        {
        }

        public OrdersController(eCommerceUserManager userManager)
        {
            UserManager = userManager;
        }

        public eCommerceUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<eCommerceUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string userID, int? orderID, int? orderStatus, int? pageNo)
        {
            var recordSize = (int)RecordSizeEnums.Size10;

            OrdersListingViewModel model = new OrdersListingViewModel
            {
                UserID = userID,
                OrderID = orderID,
                OrderStatus = orderStatus
            };

            model.Orders = OrdersService.Instance.SearchOrders(model.UserID, model.OrderID, model.OrderStatus, pageNo, recordSize, count: out int ordersCount);

            model.Pager = new Pager(ordersCount, pageNo, recordSize);

            if(Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? ID)
        {
            OrderDetailsViewModel model = new OrderDetailsViewModel();

            if (ID.HasValue)
            {
                model.Order = OrdersService.Instance.GetOrderByID(ID.Value);

                if (model.Order == null) return HttpNotFound();

                if(!string.IsNullOrEmpty(model.Order.CustomerID))
                {
                    model.Customer = await UserManager.FindByIdAsync(model.Order.CustomerID);
                }
            }
            else
            {
                return HttpNotFound();
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(int ID, int? orderStatus, string note)
        {
            JsonResult result = new JsonResult();

            if(ID > 0 && orderStatus.HasValue && !string.IsNullOrEmpty(note))
            {
                var order = OrdersService.Instance.GetOrderByID(ID);

                if(order != null)
                {
                    var orderHistory = new OrderHistory() { OrderID = order.ID, OrderStatus = orderStatus.Value, Note = note, ModifiedOn = DateTime.Now };

                    var dbOperation = OrdersService.Instance.AddOrderHistory(orderHistory);

                    if (dbOperation)
                    {
                        result.Data = new { Success = true };

                        if (!order.IsGuestOrder)
                        {
                            //send order placed notification email
                            await UserManager.SendEmailAsync(order.CustomerID, EmailTextHelpers.OrderStatusUpdatedEmailSubject(AppDataHelper.CurrentLanguage.ID, order.ID, orderStatus.Value), EmailTextHelpers.OrderStatusUpdatedEmailBody(AppDataHelper.CurrentLanguage.ID, order.ID, orderStatus.Value, Url.Action("Tracking", "Orders", new { area = "", orderID = order.ID }, protocol: Request.Url.Scheme)));
                        }

                        return result;
                    }
                }
            }

            result.Data = new { Success = false, Message = "Dashboard.OrderDetails.UpdateStatus.Validation.UnableToUpdateOrderStatus".LocalizedString() };

            return result;
        }

        public ActionResult UserOrders(string userID, int? orderID, int? orderStatus, int? pageNo)
        {
            var recordSize = (int)RecordSizeEnums.Size10;

            UserOrdersViewModel model = new UserOrdersViewModel
            {
                UserID = userID,
                OrderID = orderID,
                OrderStatus = orderStatus
            };

            model.UserOrders = OrdersService.Instance.SearchOrders(model.UserID, model.OrderID, model.OrderStatus, pageNo, recordSize, count: out int userOrdersCount);
            
            model.Pager = new Pager(userOrdersCount, pageNo, recordSize);

            return PartialView("_UserOrders", model);
        }
    }    
}