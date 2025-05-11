using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Shared.Helpers
{
    public static class EmailTextHelpers
    {
        public static string AccountRegisterEmailSubject(int languageID)
        {
            return string.Format("{0} Account Registered!", ConfigurationsHelper.ApplicationName);
        }

        public static string AccountRegisterEmailBody(int languageID, string loginURL)
        {
            return string.Format("Thanks for registering your account on {0}. Your account has been created successfully. You can login to your account here: {1}", ConfigurationsHelper.ApplicationName, loginURL);
        }

        public static string OrderPlacedEmailSubject(int languageID, int orderID)
        {
            return string.Format("Order Placed Successfully. Order# {0}", orderID);
        }

        public static string OrderPlacedEmailBody(int languageID, int orderID, string orderTrackingURL)
        {
            return string.Format("Your order# {0} has been placed successfully on {1}. You can check the details of your order here: {2}. You will be updated with your order status.", orderID, ConfigurationsHelper.ApplicationName, orderTrackingURL);
        }

        public static string OrderPlacedEmailSubject_Admin(int languageID, int orderID)
        {
            return string.Format("New Order# {0} has been Placed on {1}.", orderID, ConfigurationsHelper.ApplicationName);
        }

        public static string OrderPlacedEmailBody_Admin(int languageID, int orderID, string orderDetailsURL)
        {
            return string.Format("A new order# {0} has been placed successfully on {1}. You can check the details of order here: {2}.", orderID, ConfigurationsHelper.ApplicationName, orderDetailsURL);
        }

        public static string OrderStatusUpdatedEmailSubject(int languageID, int orderID, int orderStatus)
        {
            return string.Format("Order# {0} Status updated to {1}.", orderID, ((OrderStatus)orderStatus).ToString());
        }

        public static string OrderStatusUpdatedEmailBody(int languageID, int orderID, int orderStatus, string orderTrackingURL)
        {
            return string.Format("Your order# {0} status has been updated to {1} on {2}. You can check the details of your order here: {3}.", orderID, ((OrderStatus)orderStatus).ToString(), ConfigurationsHelper.ApplicationName, orderTrackingURL);
        }
        
        public static string ContactMessageSubject_Admin()
        {
            return string.Format("A new contact us message has been received on {0}", ConfigurationsHelper.ApplicationName);
        }

        public static string ContactMessageBody_Admin(string subject, string name, string email, string message)
        {
            return string.Format("A new message has been received on {0}. Following are the details. <br> Subject: {1}<br> Name: {2}<br> Email: {3}<br> Message Details: {4}", ConfigurationsHelper.ApplicationName, subject, name, email, message);
        }
    }
}
