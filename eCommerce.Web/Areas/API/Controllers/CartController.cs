using eCommerce.Entities;
using eCommerce.Entities.APIEntities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Services;
using eCommerce.Shared.Commons;
using eCommerce.Shared.Helpers;
using eCommerce.Web.Areas.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace eCommerce.Web.Areas.API.Controllers
{
    public class CartController : APIBaseController
    {
        [HttpPost]
        public IHttpActionResult AddProductToCart(int ID, int quantity = 1)
        {
            var product = ProductsService.Instance.GetProductByID(ID);

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

                    return APIResult(HttpStatusCode.OK, new { Message = message, CartItems = SessionHelper.CartItems, CartTotal = SessionHelper.CartItems.Sum(x=>x.ProductTotal) });
                }
                else
                {
                    return APIResult(HttpStatusCode.BadRequest, new { Message = "PP.Shopping.ProductNotAvailableInSpecifiedQuantity".LocalizedString() });
                }
            }
            else
            {
                return APIResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public IHttpActionResult ApplyPromoToCart(string promoCode)
        {
            if (!string.IsNullOrEmpty(promoCode.Trim()))
            {
                var promo = PromosService.Instance.GetPromoByCode(promoCode);

                if(promo != null)
                {
                    if (promo.Value > 0 && (promo.ValidTill == null || promo.ValidTill >= DateTime.Now))
                    {
                        SessionHelper.Promo = promo;
                        SessionHelper.PromoCode = promo.Code;

                        var discountStr = string.Empty;

                        if (promo.PromoType == (int)PromoTypes.Percentage)
                        {
                            discountStr = string.Format("{0} %", promo.Value);
                        }
                        else if (promo.PromoType == (int)PromoTypes.Amount)
                        {
                            discountStr = string.Format("{0}", promo.Value);
                        }

                        return APIResult(HttpStatusCode.OK, new { Message = "PP.Cart.PromoAppliedSuccessfully".LocalizedString().Replace("{discount}", discountStr) });
                    }
                    else return APIResult(HttpStatusCode.BadRequest, new { Message = "PP.Cart.PromoExpired".LocalizedString() });
                }
            }
            
            return APIResult(HttpStatusCode.BadRequest, new { Message = "PP.Cart.InvalidPromo".LocalizedString() });
        }

        [HttpGet]
        public IHttpActionResult CartDetails()
        {
            CartItemsViewModel model = new CartItemsViewModel();

            if (SessionHelper.CartItems != null && SessionHelper.CartItems.Count > 0)
            {
                var productIDs = SessionHelper.CartItems.Select(x => x.ItemID).ToList();

                if (productIDs.Count > 0)
                {
                    var products = ProductsService.Instance.GetProductsByIDs(productIDs);

                    if(products != null && products.Count > 0)
                    {
                        foreach (var product in products)
                        {
                            var cartItem = SessionHelper.CartItems.FirstOrDefault(x=>x.ItemID == product.ID);
                            var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);
                            var productThumbnail = product.ProductPictures != null && product.ProductPictures.Count > 0 ? product.ProductPictures.FirstOrDefault(x => x.PictureID == product.ThumbnailPictureID) : null;

                            model.CartItems.Add(new CartItemEntity() {
                                Product = new CartItemProduct() { ID = product.ID, Title = currentLanguageProductRecord.Name, Picture = productThumbnail != null && productThumbnail.Picture != null && !string.IsNullOrEmpty(productThumbnail.Picture.URL) ? string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, productThumbnail.Picture.URL) : string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.DEFAULT_PICTURE) },
                                Price = cartItem.Price,
                                Quantity = cartItem.Quantity
                            });
                        }
                    }
                }
            }

            model.SubTotal = model.CartItems.Sum(x => x.ProductTotal);

            if (!string.IsNullOrEmpty(SessionHelper.PromoCode))
            {
                var promo = PromosService.Instance.GetPromoByCode(SessionHelper.PromoCode);

                if (promo != null && promo.Value > 0 && (promo.ValidTill == null || promo.ValidTill >= DateTime.Now))
                {
                    model.PromoApplied = true;

                    if (promo.PromoType == (int)PromoTypes.Percentage)
                    {
                        model.Promo = new PromoEntity() { Code = promo.Code, PromoType = PromoTypes.Percentage.ToString(), Value = promo.Value, ValidTill = promo.ValidTill };

                        model.Discount = Math.Round((model.SubTotal * promo.Value) / 100);
                    }
                    else if (promo.PromoType == (int)PromoTypes.Amount)
                    {
                        model.Promo = new PromoEntity() { Code = promo.Code, PromoType = PromoTypes.Amount.ToString(), Value = promo.Value, ValidTill = promo.ValidTill };

                        model.Discount = promo.Value;
                    }
                }
            }

            model.DeliveryCharges = ConfigurationsHelper.FlatDeliveryCharges;
            model.FinalTotal = model.SubTotal - model.Discount + model.DeliveryCharges;

            return APIResult(HttpStatusCode.OK, model);
        }


        [HttpPost]
        public IHttpActionResult ClearCart()
        {
            SessionHelper.ClearCart();

            return APIResult(HttpStatusCode.OK);
        }
    }
}