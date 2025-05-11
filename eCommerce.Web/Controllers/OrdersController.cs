using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using eCommerce.Shared.Enums;
using eCommerce.Entities.CustomEntities;
using System.Threading;

namespace eCommerce.Web.Controllers
{
    public class OrdersController : PublicBaseController
    {
        private eCommerceSignInManager _signInManager;
        private eCommerceUserManager _userManager;
        private eCommerceRoleManager _roleManager;

        public eCommerceSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<eCommerceSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<eCommerceUserManager>();
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
                return _roleManager ?? HttpContext.GetOwinContext().Get<eCommerceRoleManager>();
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

        public async Task<JsonResult> PlaceOrder(PlaceOrderCrediCardViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
                {
                    model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs);
                    }
                }

                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0 && model.Products != null && model.Products.Count > 0)
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
                                await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, Url.Action("Login", "Users", null, protocol: Request.Url.Scheme)));

                                newOrder.CustomerID = user.Id;
                            }
                            else
                            {
                                jsonResult.Data = new
                                {
                                    Success = false,
                                    Message = string.Join("<br />", result.Errors)
                                };
                                return jsonResult;
                            }
                        }
                        catch
                        {
                            jsonResult.Data = new
                            {
                                Success = false,
                                Message = string.Format("An error occured while registering user.")
                            };
                            return jsonResult;
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
                    foreach (var product in SessionHelper.CartItems.Select(x=>model.Products.FirstOrDefault(y=>y.ID == x.ItemID)))
                    {
                        var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                        var orderItem = new OrderItem
                        {
                            ProductID = product.ID,
                            ProductName = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Format("Product ID : {0}", product.ID),
                            ItemPrice = product.Price,
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
                    newOrder.PaymentMethod = (int)PaymentMethods.CreditCard;

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
                                await UserManager.SendEmailAsync(newOrder.CustomerID, EmailTextHelpers.OrderPlacedEmailSubject(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Tracking", "Orders", new { orderID = newOrder.ID }, protocol: Request.Url.Scheme)));

                                //send order placed notification email to admin emails
                                await new EmailService().SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName, ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.AdminEmailAddress, EmailTextHelpers.OrderPlacedEmailSubject_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Details", "Orders", new { area = "Dashboard", ID = newOrder.ID }, protocol: Request.Url.Scheme)));
                            }

                            jsonResult.Data = new
                            {
                                Success = true,
                                OrderID = newOrder.ID
                            };
                        }
                        else
                        {
                            jsonResult.Data = new
                            {
                                Success = false,
                                Message = "System can not take any order."
                            };
                        }
                    }
                    else
                    {
                        jsonResult.Data = new
                        {
                            Success = authorizeNetResponse.Success,
                            Message = authorizeNetResponse.Message
                        };
                    }
                }
                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = string.Join("\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                };
            }
            
            return jsonResult;
        }

        public async Task<JsonResult> PlaceOrderViaCashOnDelivery(PlaceOrderCashOnDeliveryViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
                {
                    model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs);
                    }
                }

                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var newOrder = new Order();

                    if(User.Identity.IsAuthenticated)
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
                                await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, Url.Action("Login", "Users", null, protocol: Request.Url.Scheme)));
                                
                                newOrder.CustomerID = user.Id;
                            }
                            else
                            {
                                jsonResult.Data = new
                                {
                                    Success = false,
                                    Message = string.Join("<br />", result.Errors)
                                };
                                return jsonResult;
                            }
                        }
                        catch
                        {
                            jsonResult.Data = new
                            {
                                Success = false,
                                Message = string.Format("An error occured while registering user.")
                            };
                            return jsonResult;
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
                    foreach (var product in SessionHelper.CartItems.Select(x => model.Products.FirstOrDefault(y => y.ID == x.ItemID)))
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
                            await UserManager.SendEmailAsync(newOrder.CustomerID, EmailTextHelpers.OrderPlacedEmailSubject(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Tracking", "Orders", new { orderID = newOrder.ID }, protocol: Request.Url.Scheme)));

                            //send order placed notification email to admin emails
                            await new EmailService().SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName, ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.AdminEmailAddress, EmailTextHelpers.OrderPlacedEmailSubject_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Details", "Orders", new { area = "Dashboard",  ID = newOrder.ID }, protocol: Request.Url.Scheme)));
                        }

                        jsonResult.Data = new
                        {
                            Success = true,
                            OrderID = newOrder.ID
                        };
                    }
                    else
                    {
                        jsonResult.Data = new
                        {
                            Success = false,
                            Message = "System can not take any order."
                        };
                    }
                }
                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = string.Join("\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                };
            }

            return jsonResult;
        }

        public async Task<JsonResult> PlaceOrderViaPayPal(PlaceOrderPayPalViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
                {
                    model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs);
                    }
                }

                if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0 && model.Products != null && model.Products.Count > 0)
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
                                await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, Url.Action("Login", "Users", null, protocol: Request.Url.Scheme)));

                                newOrder.CustomerID = user.Id;
                            }
                            else
                            {
                                jsonResult.Data = new
                                {
                                    Success = false,
                                    Message = string.Join("<br />", result.Errors)
                                };
                                return jsonResult;
                            }
                        }
                        catch
                        {
                            jsonResult.Data = new
                            {
                                Success = false,
                                Message = string.Format("An error occured while registering user.")
                            };
                            return jsonResult;
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
                    foreach (var product in SessionHelper.CartItems.Select(x => model.Products.FirstOrDefault(y => y.ID == x.ItemID)))
                    {
                        var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                        var orderItem = new OrderItem
                        {
                            ProductID = product.ID,
                            ProductName = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Format("Product ID : {0}", product.ID),
                            ItemPrice = product.Price,
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
                    newOrder.PaymentMethod = (int)PaymentMethods.PayPal;
                    newOrder.TransactionID = model.TransactionID;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = string.Format("Order placed via PayPal by PayPal Account Name: {0} ({1})", model.AccountName, model.AccountEmail)
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
                            await UserManager.SendEmailAsync(newOrder.CustomerID, EmailTextHelpers.OrderPlacedEmailSubject(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Tracking", "Orders", new { orderID = newOrder.ID }, protocol: Request.Url.Scheme)));

                            //send order placed notification email to admin emails
                            await new EmailService().SendToEmailAsync(ConfigurationsHelper.SendGrid_FromEmailAddressName, ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.AdminEmailAddress, EmailTextHelpers.OrderPlacedEmailSubject_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID), EmailTextHelpers.OrderPlacedEmailBody_Admin(AppDataHelper.CurrentLanguage.ID, newOrder.ID, Url.Action("Details", "Orders", new { area = "Dashboard", ID = newOrder.ID }, protocol: Request.Url.Scheme)));
                        }

                        jsonResult.Data = new
                        {
                            Success = true,
                            OrderID = newOrder.ID
                        };
                    }
                    else
                    {
                        jsonResult.Data = new
                        {
                            Success = false,
                            Message = "System can not take any order."
                        };
                    }
                }
                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = string.Join("\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                };
            }

            return jsonResult;
        }

        public ActionResult Tracking(int? orderID, bool orderPlaced = false)
        {
            TrackOrderViewModel model = new TrackOrderViewModel
            {
                OrderID = orderID
            };

            if (orderID.HasValue)
            {
                model.Order = OrdersService.Instance.GetOrderByID(orderID.Value);
            }

            ViewBag.ShowOrderPlaceMessage = orderPlaced;

            return View(model);
        }

        public ActionResult UserOrders(string userID, int? orderID, int? orderStatus, int? pageNo)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            UserOrdersViewModel model = new UserOrdersViewModel
            {
                UserID = !string.IsNullOrEmpty(userID) ? userID : User.Identity.GetUserId(),
                OrderID = orderID,
                OrderStatus = orderStatus
            };

            model.UserOrders = OrdersService.Instance.SearchOrders(model.UserID, model.OrderID, model.OrderStatus, pageNo, pageSize, count: out int ordersCount);

            model.Pager = new Pager(ordersCount, pageNo, pageSize);

            return PartialView("_UserOrders", model);
        }

        public ActionResult PrintInvoice(int orderID)
        {
            PrintInvoiceViewModel model = new PrintInvoiceViewModel
            {
                OrderID = orderID,

                Order = OrdersService.Instance.GetOrderByID(orderID)
            };

            if (model.Order == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PrintInvoice", model);
        }
    }
}