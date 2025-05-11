using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Shared.Helpers
{
    public static class SessionHelper
    {
        private const string CART = "CART";
        private const string CART_ITEMS = "CART_ITEMS";
        private const string PROMO = "PROMO";
        private const string PROMO_CODE = "PROMO_CODE";
        private const string DARK_MODE = "DARK_MODE";

        public static Cart Cart
        {
            get {
                var cart = SessionManager.Get<Cart>(CART);

                if(cart == null)
                {
                    cart = new Cart();

                    SessionManager.Set(CART, cart);
                }

                return cart;
            }
            set { SessionManager.Set(CART, value); }
        }

        public static List<CartItem> CartItems
        {
            get
            {
                var cartItems = SessionManager.Get<List<CartItem>>(CART_ITEMS);

                if(cartItems == null || cartItems.Count == 0)
                {
                    cartItems = cartItems == null ? new List<CartItem>() : cartItems;

                    SessionManager.Set(CART_ITEMS, cartItems);
                }

                return cartItems;
            }
            set { SessionManager.Set(CART_ITEMS, value); }
        }

        public static Promo Promo
        {
            get
            {
                return SessionManager.Get<Promo>(PROMO);
            }
            set { SessionManager.Set(PROMO, value); }
        }
        public static string PromoCode
        {
            get
            {
                var promoCode = SessionManager.Get<string>(PROMO_CODE);

                if (string.IsNullOrEmpty(promoCode))
                {
                    promoCode = string.Empty;

                    SessionManager.Set(PROMO_CODE, promoCode);
                }

                return promoCode;
            }
            set { SessionManager.Set(PROMO_CODE, value); }
        }

        public static void ClearCart()
        {
            CartItems.Clear();
            PromoCode = string.Empty;
            Promo = null;
        }

        public static string DarkMode
        {
            get
            {
                var darkMode = SessionManager.Get<string>(DARK_MODE);

                if (string.IsNullOrEmpty(darkMode))
                {
                    darkMode = "false";

                    SessionManager.Set(DARK_MODE, darkMode);
                }

                return darkMode;
            }
            set { SessionManager.Set(DARK_MODE, value); }
        }
    }
}
