using AuthorizeNet.Api.Contracts.V1;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.Areas.API.Commons.ActionResults;
using eCommerce.Web.Areas.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrderItem = eCommerce.Entities.OrderItem;

namespace eCommerce.Web.Areas.API.Controllers
{
    public class OrdersController : APIBaseController
    {
        private eCommerceSignInManager _signInManager;
        private eCommerceUserManager _userManager;
        private eCommerceRoleManager _roleManager;

        public eCommerceSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().GetUserManager<eCommerceSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public eCommerceUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<eCommerceUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public eCommerceRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<eCommerceRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public OrdersController()
        {
        }

        public OrdersController(eCommerceUserManager userManager, eCommerceSignInManager signInManager, eCommerceRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public APIResult Tracking(int orderID)
        {
            var order = OrdersService.Instance.GetOrderByID(orderID);

            if (order != null)
            {
                if (order.IsGuestOrder || (User.Identity.IsAuthenticated && (order.CustomerID == User.Identity.GetUserId() || User.IsInRole("Administrator"))))
                {
                    var orderTracking = new OrderTracking();

                    var status = order.OrderHistory != null && order.OrderHistory.Count > 0 ? order.OrderHistory.OrderByDescending(x => x.ModifiedOn).FirstOrDefault() : new OrderHistory();

                    orderTracking.Status = string.Format("PP.Tracking.OrderStatus.{0}", ((OrderStatus)status.OrderStatus).ToString()).LocalizedString();
                    orderTracking.PaymentMethod = string.Format("PP.Tracking.PaymentMethods.{0}{1}", ((PaymentMethods)order.PaymentMethod).ToString(), string.Format("{0}", !string.IsNullOrEmpty(order.TransactionID) ? "PP.Tracking.TransactionID".LocalizedString() + ": " + order.TransactionID : string.Empty)).LocalizedString();
                    orderTracking.TransactionID = order.TransactionID;
                    orderTracking.PlacedOn = order.PlacedOn.ToString();
                    orderTracking.UpdatedOn = status != null ? status.ModifiedOn.ToString() : string.Empty;
                    orderTracking.UniqueCode = order.OrderCode;

                    if (order.OrderItems != null && order.OrderItems.Count > 0)
                    {
                        var orderItems = new List<Models.OrderItem>();

                        foreach (var orderItem in order.OrderItems)
                        {
                            var currentLanguageProductRecord = orderItem.Product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                            orderItems.Add(new Models.OrderItem()
                            {
                                ID = orderItem.ProductID,
                                ProductName = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Empty,
                                Price = orderItem.ItemPrice,
                                Quantity = orderItem.Quantity,
                                ProductTotal = orderItem.ItemPrice * orderItem.Quantity
                            });
                        }

                        orderTracking.OrderDetails = new OrderDetails()
                        {
                            OrderItems = orderItems,
                            TotalAmmount = order.TotalAmmount,
                            Discount = order.Discount,
                            PromoDetails = order.Promo != null ? "PP.Tracking.PromoApplied".LocalizedString().Replace("{promocode}", order.Promo.Code) : string.Empty,
                            DeliveryCharges = order.DeliveryCharges,
                            FinalAmmount = order.FinalAmmount
                        };
                    }

                    orderTracking.ContactDetails = new ContactDetails()
                    {
                        GuestOrder = order.IsGuestOrder,
                        CustomerID = order.CustomerID,
                        CustomerName = order.CustomerName,
                        CustomerEmail = order.CustomerEmail.HideEmail(),
                        CustomerPhone = order.CustomerPhone
                    };

                    orderTracking.ShippingDetails = new ShippingDetails()
                    {
                        CustomerAddress = order.CustomerAddress,
                        CustomerCity = order.CustomerCity,
                        CustomerCountry = order.CustomerCountry,
                        CustomerZipCode = order.CustomerZipCode
                    };

                    orderTracking.OrderHistory = order.OrderHistory != null ? order.OrderHistory.OrderByDescending(x=>x.ModifiedOn).Select(x => new OrderHistoryRecord() { ModifiedOn = x.ModifiedOn.HasValue ? x.ModifiedOn.ToString() : string.Empty, Note = x.Note, Status = string.Format("PP.Tracking.OrderStatus." + ((OrderStatus)x.OrderStatus).ToString()).LocalizedString() }).ToList() : null;

                    return APIResult(HttpStatusCode.OK, orderTracking);
                }
                else return APIResult(HttpStatusCode.Unauthorized);
            }
            else return APIResult(HttpStatusCode.NotFound);
        }

        [HttpPost]
        public async Task<APIResult> PlaceOrderViaCashOnDelivery(PlaceOrderModel model)
        {
            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                var cartProducts = new List<Product>();

                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
                {
                    var productIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                    if (productIDs.Count > 0)
                    {
                        cartProducts = ProductsService.Instance.GetProductsByIDs(productIDs);
                    }
                }

                if (cartProducts != null && cartProducts.Count > 0)
                {
                    var newOrder = new Order();

                    if (User.Identity.IsAuthenticated)
                    {
                        newOrder.CustomerID = User.Identity.GetUserId();
                    }
                    else if (model.CreateAccount)
                    {
                        try
                        {
                            var user = new eCommerceUser { FullName = model.FullName, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, RegisteredOn = DateTime.Now };

                            var result = await UserManager.CreateAsync(user);

                            if (result.Succeeded)
                            {
                                if (await RoleManager.RoleExistsAsync("User"))
                                {
                                    //assign User role to newly registered user
                                    await UserManager.AddToRoleAsync(user.Id, "User");
                                }

                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                                //send account register notification email
                                await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, this.Url.Link("Login", null)));

                                newOrder.CustomerID = user.Id;
                            }
                            else
                            {
                                return APIResult(HttpStatusCode.BadRequest, new { Message = string.Join(Environment.NewLine, result.Errors) });
                            }
                        }
                        catch
                        {
                            return APIResult(HttpStatusCode.InternalServerError, new { Message = string.Format("An error occured while registering user.") });
                        }
                    }
                    else
                    {
                        newOrder.IsGuestOrder = true;
                    }

                    newOrder.CustomerName = model.FullName;
                    newOrder.CustomerEmail = model.Email;
                    newOrder.CustomerPhone = model.PhoneNumber;
                    newOrder.CustomerCountry = model.Country;
                    newOrder.CustomerCity = model.City;
                    newOrder.CustomerAddress = model.Address;
                    newOrder.CustomerZipCode = model.ZipCode;

                    newOrder.OrderItems = new List<OrderItem>();
                    foreach (var product in SessionHelper.CartItems.Select(x => cartProducts.FirstOrDefault(y => y.ID == x.ItemID)))
                    {
                        var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                        var orderItem = new OrderItem
                        {
                            ProductID = product.ID,
                            ProductName = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Format("Product ID : {0}", product.ID),

                            ItemPrice = product.Discount.HasValue && product.Discount.Value > 0 ? product.Discount.Value : product.Price,

                            Quantity = SessionHelper.CartItems.FirstOrDefault(x => x.ItemID == product.ID).Quantity
                        };

                        newOrder.OrderItems.Add(orderItem);
                    }

                    newOrder.OrderCode = Guid.NewGuid().ToString();
                    newOrder.TotalAmmount = newOrder.OrderItems.Sum(x => (x.ItemPrice * x.Quantity));
                    newOrder.DeliveryCharges = ConfigurationsHelper.FlatDeliveryCharges;

                    //Applying Promo/voucher 
                    if (!string.IsNullOrEmpty(SessionHelper.PromoCode))
                    {
                        var promo = SessionHelper.Promo;
                        if (promo != null && promo.Value > 0)
                        {
                            if (promo.ValidTill == null || promo.ValidTill >= DateTime.Now)
                            {
                                newOrder.PromoID = promo.ID;

                                if (promo.PromoType == (int)PromoTypes.Percentage)
                                {
                                    newOrder.Discount = Math.Round((newOrder.TotalAmmount * promo.Value) / 100);
                                }
                                else if (promo.PromoType == (int)PromoTypes.Amount)
                                {
                                    newOrder.Discount = promo.Value;
                                }
                            }
                        }
                    }

                    newOrder.FinalAmmount = newOrder.TotalAmmount + newOrder.DeliveryCharges - newOrder.Discount;
                    newOrder.PaymentMethod = (int)PaymentMethods.CashOnDelivery;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;

                    if (OrdersService.Instance.SaveOrder(newOrder))
                    {
                        SessionHelper.ClearCart();

                        ProductsService.Instance.UpdateProductQuantities(newOrder);

                        if (!newOrder.IsGuestOrder)
                        {
                            //send order placed notification email
                            await UserManager.SendEmailAsync(newOrder.CustomerID, EmailTextHelpers.OrderPlacedEmailSubject(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody(AppDataHelper.CurrentLanguage.ID, newOrder.ID, this.Url.Link("OrderTrack", new { orderID = newOrder.ID })));

                            //send order placed notification email to admin emails
                            await new EmailService().SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName, ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.AdminEmailAddress, EmailTextHelpers.OrderPlacedEmailSubject_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID, this.Url.Link("EntityDetails", new { controller = "Orders", area = "Dashboard", ID = newOrder.ID })));
                        }

                        return APIResult(HttpStatusCode.OK, new { Message = "PP.Checkout.OrderPlacedSuccessfully".LocalizedString(), OrderID = newOrder.ID });
                    }
                    else
                    {
                        return APIResult(HttpStatusCode.InternalServerError, new { Message = "PP.Message.GenericErrorMessage".LocalizedString() });
                    }
                }
                else
                {
                    return APIResult(HttpStatusCode.BadRequest, new { Message = "Invalid products in cart." });
                }
            }
            else
            {
                return APIResult(HttpStatusCode.BadRequest, new
                {
                    Message = string.Join("\n", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage))
                });
            }
        }

        [HttpPost]
        public async Task<APIResult> PlaceOrderCreditCard(PlaceOrderCrediCardModel model)
        {
            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                var cartProducts = new List<Product>();

                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
                {
                    var productIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                    if (productIDs.Count > 0)
                    {
                        cartProducts = ProductsService.Instance.GetProductsByIDs(productIDs);
                    }
                }

                if (cartProducts != null && cartProducts.Count > 0)
                {
                    var newOrder = new Order();

                    if (User.Identity.IsAuthenticated)
                    {
                        newOrder.CustomerID = User.Identity.GetUserId();
                    }
                    else if (model.CreateAccount)
                    {
                        try
                        {
                            var user = new eCommerceUser { FullName = model.FullName, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, RegisteredOn = DateTime.Now };

                            var result = await UserManager.CreateAsync(user);

                            if (result.Succeeded)
                            {
                                if (await RoleManager.RoleExistsAsync("User"))
                                {
                                    //assign User role to newly registered user
                                    await UserManager.AddToRoleAsync(user.Id, "User");
                                }

                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                                //send account register notification email
                                await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, this.Url.Link("Login", null)));

                                newOrder.CustomerID = user.Id;
                            }
                            else
                            {
                                return APIResult(HttpStatusCode.BadRequest, new { Message = string.Join(Environment.NewLine, result.Errors) });
                            }
                        }
                        catch
                        {
                            return APIResult(HttpStatusCode.InternalServerError, new { Message = string.Format("An error occured while registering user.") });
                        }
                    }
                    else
                    {
                        newOrder.IsGuestOrder = true;
                    }

                    newOrder.CustomerName = model.FullName;
                    newOrder.CustomerEmail = model.Email;
                    newOrder.CustomerPhone = model.PhoneNumber;
                    newOrder.CustomerCountry = model.Country;
                    newOrder.CustomerCity = model.City;
                    newOrder.CustomerAddress = model.Address;
                    newOrder.CustomerZipCode = model.ZipCode;

                    newOrder.OrderItems = new List<OrderItem>();
                    foreach (var product in SessionHelper.CartItems.Select(x => cartProducts.FirstOrDefault(y => y.ID == x.ItemID)))
                    {
                        var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                        var orderItem = new OrderItem
                        {
                            ProductID = product.ID,
                            ProductName = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Format("Product ID : {0}", product.ID),

                            ItemPrice = product.Discount.HasValue && product.Discount.Value > 0 ? product.Discount.Value : product.Price,

                            Quantity = SessionHelper.CartItems.FirstOrDefault(x => x.ItemID == product.ID).Quantity
                        };

                        newOrder.OrderItems.Add(orderItem);
                    }

                    newOrder.OrderCode = Guid.NewGuid().ToString();
                    newOrder.TotalAmmount = newOrder.OrderItems.Sum(x => (x.ItemPrice * x.Quantity));
                    newOrder.DeliveryCharges = ConfigurationsHelper.FlatDeliveryCharges;

                    //Applying Promo/voucher 
                    if (!string.IsNullOrEmpty(SessionHelper.PromoCode))
                    {
                        var promo = SessionHelper.Promo;
                        if (promo != null && promo.Value > 0)
                        {
                            if (promo.ValidTill == null || promo.ValidTill >= DateTime.Now)
                            {
                                newOrder.PromoID = promo.ID;

                                if (promo.PromoType == (int)PromoTypes.Percentage)
                                {
                                    newOrder.Discount = Math.Round((newOrder.TotalAmmount * promo.Value) / 100);
                                }
                                else if (promo.PromoType == (int)PromoTypes.Amount)
                                {
                                    newOrder.Discount = promo.Value;
                                }
                            }
                        }
                    }

                    newOrder.FinalAmmount = newOrder.TotalAmmount + newOrder.DeliveryCharges - newOrder.Discount;
                    newOrder.PaymentMethod = (int)PaymentMethods.CashOnDelivery;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;

                    #region ExecuteAuthorizeNetPayment Execution

                    var creditCardInfo = new creditCardType
                    {
                        cardNumber = model.CCCardNumber,
                        expirationDate = string.Format("{0}{1}", model.CCExpiryMonth, model.CCExpiryYear),
                        cardCode = model.CCCVC
                    };

                    var authorizeNetResponse = AuthorizeNetHelper.ExecutePayment(newOrder, creditCardInfo);

                    #endregion


                    if (authorizeNetResponse.Success)
                    {
                        newOrder.TransactionID = authorizeNetResponse.Response.transactionResponse.transId;

                        if (OrdersService.Instance.SaveOrder(newOrder))
                        {
                            SessionHelper.ClearCart();

                            ProductsService.Instance.UpdateProductQuantities(newOrder);

                            if (!newOrder.IsGuestOrder)
                            {
                                //send order placed notification email
                                await UserManager.SendEmailAsync(newOrder.CustomerID, EmailTextHelpers.OrderPlacedEmailSubject(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody(AppDataHelper.CurrentLanguage.ID, newOrder.ID, this.Url.Link("OrderTrack", new { orderID = newOrder.ID })));

                                //send order placed notification email to admin emails
                                await new EmailService().SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName, ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.AdminEmailAddress, EmailTextHelpers.OrderPlacedEmailSubject_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID, this.Url.Link("EntityDetails", new { controller = "Orders", area = "Dashboard", ID = newOrder.ID })));
                            }

                            return APIResult(HttpStatusCode.OK, new { Message = "PP.Checkout.OrderPlacedSuccessfully".LocalizedString(), OrderID = newOrder.ID });
                        }
                        else
                        {
                            return APIResult(HttpStatusCode.InternalServerError, new { Message = "PP.Message.GenericErrorMessage".LocalizedString() });
                        }
                    }
                    else
                    {
                        return APIResult(HttpStatusCode.InternalServerError, new { Message = authorizeNetResponse.Message });
                    }
                }
                else
                {
                    return APIResult(HttpStatusCode.BadRequest, new { Message = "Invalid products in cart." });
                }
            }
            else
            {
                return APIResult(HttpStatusCode.BadRequest, new
                {
                    Message = string.Join("\n", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage))
                });
            }
        }
    }
}