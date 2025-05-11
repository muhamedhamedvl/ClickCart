using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Controllers
{
    public class CartController : PublicBaseController
    {
        private eCommerceSignInManager _signInManager;
        private eCommerceUserManager _userManager;

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

        public CartController()
        {
        }

        public CartController(eCommerceUserManager userManager, eCommerceSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ActionResult Cart(bool isPopup = false)
        {
            CartItemsViewModel model = new CartItemsViewModel();

            if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
            {
                model.CartItems = SessionHelper.CartItems.OrderByDescending(x=>x.ItemID).ToList();
                model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs);
                }
            }

            if (!string.IsNullOrEmpty(SessionHelper.PromoCode))
            {
                var promo = PromosService.Instance.GetPromoByCode(SessionHelper.PromoCode);

                if (promo != null && promo.Value > 0 && (promo.ValidTill == null || promo.ValidTill >= DateTime.Now))
                {
                    model.PromoCode = promo.Code;
                    model.Promo = promo;
                }
            }

            if (isPopup)
            {
                return PartialView("_CartPopup", model);
            }
            else
            {
                if(Request.IsAjaxRequest())
                {
                    return PartialView("_CartItems", model);
                }
                else return View("CartItems", model);
            }
        }

        public ActionResult UpdateCart(FormCollection formCollection)
        {
            var cartItemsUpdate = GetCartItemUpdateFromForm(formCollection);

            CartItemsViewModel model = new CartItemsViewModel();
            
            if (SessionHelper.CartItems != null)
            {
                var productIDs = cartItemsUpdate.CartItems != null &&
                                   cartItemsUpdate.CartItems.Count > 0 ?
                                   cartItemsUpdate.CartItems.Select(x => x.ItemID).ToList() : new List<int>();

                if (productIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(productIDs);
                }

                SessionHelper.CartItems.Clear();

                if (model.Products != null && model.Products.Count > 0)
                {
                    foreach (var product in model.Products)
                    {
                        var productPrice = product.Discount.HasValue && product.Discount.Value > 0 ? product.Discount.Value : product.Price;

                        SessionHelper.CartItems.Add(new CartItem() { ItemID = product.ID, Price = productPrice, Quantity = cartItemsUpdate.CartItems.FirstOrDefault(x => x.ItemID == product.ID).Quantity });
                    }
                }

                model.CartItems = SessionHelper.CartItems.OrderByDescending(x => x.ItemID).ToList();
                model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();
            }

            if (!string.IsNullOrEmpty(cartItemsUpdate.PromoCode))
            {
                var promo = PromosService.Instance.GetPromoByCode(cartItemsUpdate.PromoCode);

                if (promo != null && promo.Value > 0 && (promo.ValidTill == null || promo.ValidTill >= DateTime.Now))
                {
                    SessionHelper.Promo = promo;
                    SessionHelper.PromoCode = promo.Code;

                    model.Promo = promo;
                }

                model.PromoCode = cartItemsUpdate.PromoCode;
            }

            return PartialView("_CartItems", model);
        }

        private UpdateCartItemsViewModel GetCartItemUpdateFromForm(FormCollection formCollection)
        {
            var model = new UpdateCartItemsViewModel
            {
                CartItems = new List<CartItem>()
            };

            foreach (string key in formCollection)
            {
                if (key.Contains("product_"))
                {
                    var value = formCollection[key];

                    if (!string.IsNullOrEmpty(value))
                    {
                        var cartItem = new CartItem
                        {
                            ItemID = int.TryParse(key.GetSubstringText("_", ""), out int productID) ? productID : 0,

                            Quantity = int.TryParse(value, out int quantity) ? quantity : 0
                        };

                        model.CartItems.Add(cartItem);
                    }
                }
                else if (key.Contains("promo"))
                {
                    var value = formCollection[key];

                    if (!string.IsNullOrEmpty(value))
                    {
                        model.PromoCode = value;
                    }
                }
            }

            return model;
        }

        public JsonResult AddItemToCart(int itemID, int quantity = 1)
        {
            JsonResult json = new JsonResult();

            var product = ProductsService.Instance.GetProductByID(itemID);

            if (product != null)
            {
                if (product.StockQuantity > 0 && product.StockQuantity >= quantity)
                {
                    var message = string.Empty;

                    var itemInCart = SessionHelper.CartItems.FirstOrDefault(x => x.ItemID == product.ID);

                    if (itemInCart != null)
                    {
                        //update cart item quantity.
                        itemInCart.Quantity += quantity;
                        message = "PP.Shopping.CartItemQuantityUpdated".LocalizedString();
                    }
                    else
                    {
                        var productPrice = product.Discount.HasValue && product.Discount.Value > 0 ? product.Discount.Value : product.Price;

                        //add the product to cart.
                        SessionHelper.CartItems.Add(new CartItem() { ItemID = product.ID, Price = productPrice, Quantity = quantity });
                        message = "PP.Shopping.ItemAddedToCart".LocalizedString();
                    }

                    json.Data = new { Success = true, Message = message, CartItems = SessionHelper.CartItems };
                }
                else
                {
                    json.Data = new { Success = false, Message = "PP.Shopping.ProductNotAvailableInSpecifiedQuantity".LocalizedString() };
                }
            }
            else
            {
                json.Data = new { Success = false, Message = "PP.Shopping.ItemNotFound".LocalizedString() };
            }

            return json;
        }

        public JsonResult GetCartItems()
        {
            JsonResult json = new JsonResult
            {
                Data = new { Success = true, CartItems = SessionHelper.CartItems }
            };

            return json;
        }

        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
            {
                model.CartItems = SessionHelper.CartItems.OrderByDescending(x => x.ItemID).ToList();
                model.ProductIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs);
                }

                model.TotalAmount = SessionHelper.CartItems.Sum(z => z.ProductTotal);
            }

            if (!string.IsNullOrEmpty(SessionHelper.PromoCode))
            {
                var promo = PromosService.Instance.GetPromoByCode(SessionHelper.PromoCode);

                if (promo != null && promo.Value > 0 && (promo.ValidTill == null || promo.ValidTill >= DateTime.Now))
                {
                    model.PromoCode = promo.Code;
                    model.Promo = promo;

                    if (model.Promo.PromoType == (int)PromoTypes.Percentage)
                    {
                        model.Discount = Math.Round((model.TotalAmount * model.Promo.Value) / 100);
                    }
                    else if (model.Promo.PromoType == (int)PromoTypes.Amount)
                    {
                        model.Discount = SessionHelper.Promo.Value;
                    }

                    model.PromoApplied = true;
                }
            }

            model.CartHasProducts = model.Products != null && model.Products.Count > 0;

            if(model.CartHasProducts)
            {
                model.FinalAmount = model.TotalAmount - model.Discount + ConfigurationsHelper.FlatDeliveryCharges;
            }

            if (User.Identity.IsAuthenticated)
            {
                model.User = UserManager.FindById(User.Identity.GetUserId());
            }
            else
            {
                model.User = new eCommerceUser();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Checkout", model);
            }
            else
            {
                return View(model);
            }
        }
    }
}