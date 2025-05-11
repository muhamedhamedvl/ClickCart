using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.API.Models
{
    public class OrderTracking
    {
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public string PlacedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UniqueCode { get; set; }

        public OrderDetails OrderDetails { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
        public List<OrderHistoryRecord> OrderHistory { get; set; }
    }

    public class OrderDetails
    {
        public List<OrderItem> OrderItems { get; set; }

        public decimal TotalAmmount { get; set; }
        public decimal Discount { get; set; }
        public string PromoDetails { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal FinalAmmount { get; set; }
    }

    public class OrderItem
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotal { get; set; }
    }

    public class ContactDetails
    {
        public bool GuestOrder { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class ShippingDetails
    {
        public string CustomerCountry { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerZipCode { get; set; }
    }

    public class OrderHistoryRecord
    {
        public string ModifiedOn { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }

    public class OrdersListModel
    {
        public OrdersListModel()
        {
            OrdersListFilters = new OrdersListFilters();
            Orders = new List<UserOrder>();
        }

        public int TotalComments { get; set; }
        public int TotalPages { get; set; }
        public OrdersListFilters OrdersListFilters { get; set; }

        public List<UserOrder> Orders { get; set; }
    }

    public class OrdersListFilters
    {
        public int PageNo { get; set; }
        public int RecordSize { get; set; }
    }

    public class UserOrder
    {
        public int ID { get; set; }
        public DateTime PlacedOn { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal FinalAmmount { get; set; }
    }

    public class PlaceOrderModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public bool CreateAccount { get; set; }
    }

    public class PlaceOrderCrediCardModel : AuthorizeNetCreditCardModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public bool CreateAccount { get; set; }
    }
}