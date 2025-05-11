using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCommerce.Web.ViewModels
{
    public class TrackOrderViewModel : PageViewModel
    {
        public int? OrderID { get; set; }
        public string CustomerEmail { get; set; }
        public Order Order { get; set; }
    }

    public class PrintInvoiceViewModel : PageViewModel
    {
        public Order Order { get; set; }
        public int? OrderID { get; set; }
    }

    public class PlaceOrderCrediCardViewModel : AuthorizeNetCreditCardModel
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Customer Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        public int PromoID { get; set; }
        public decimal Discount { get; set; }

        public bool CreateAccount { get; set; }

        public List<int> ProductIDs { get; set; }
        public List<Product> Products { get; set; }
    }

    public class PlaceOrderCashOnDeliveryViewModel
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Customer Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        public int PromoID { get; set; }
        public decimal Discount { get; set; }

        public bool CreateAccount { get; set; }

        public List<int> ProductIDs { get; set; }
        public List<Product> Products { get; set; }
    }

    public class PlaceOrderPayPalViewModel : PlaceOrderCashOnDeliveryViewModel
    {
        public string TransactionID { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
    }

    public class UserOrdersViewModel
    {
        public int? OrderID { get; set; }
        public int? OrderStatus { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public eCommerceUser User { get; set; }
        public List<Order> UserOrders { get; set; }
        public Pager Pager { get; set; }
    }
}