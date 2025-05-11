using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using eCommerce.Entities;
using eCommerce.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace eCommerce.Shared.Helpers
{
    public static class AuthorizeNetHelper
    {
        private static string _ApiLoginID = "";
        private static string _ApiTransactionKey = "";
        private static bool _SandBox = true;

        public static void ConfigureAuthorizeNetAPI()
        {
            _ApiLoginID = ConfigurationsHelper.AuthorizeNet_ApiLoginID;
            _ApiTransactionKey = ConfigurationsHelper.AuthorizeNet_ApiTransactionKey;
            _SandBox = ConfigurationsHelper.AuthorizeNet_SandBox;

            if (_SandBox)
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            }
            else
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
            }

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = _ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _ApiTransactionKey,
            };
        }

        public static AuthorizeNetResponse ExecutePayment(Order newOrder, creditCardType creditCardInfo)
        {
            ConfigureAuthorizeNetAPI();

            AuthorizeNetResponse authorizeNetResponse = new AuthorizeNetResponse();

            try
            {
                var billingAddress = new customerAddressType
                {
                    firstName = newOrder.CustomerName,
                    email = newOrder.CustomerEmail,
                    phoneNumber = newOrder.CustomerPhone,
                    country = newOrder.CustomerCountry,
                    city = newOrder.CustomerCity,
                    address = newOrder.CustomerAddress,
                    zip = newOrder.CustomerZipCode
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCardInfo };


                var lineItems = new lineItemType[newOrder.OrderItems.Count];
                var i = 0;
                foreach (var orderItem in newOrder.OrderItems)
                {
                    lineItems[i] = new lineItemType { itemId = orderItem.ProductID.ToString(), name = orderItem.ProductName.ToAuthorizeNetProductName(), quantity = orderItem.Quantity, unitPrice = orderItem.ItemPrice };

                    i++;
                }
                
                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                    amount = newOrder.FinalAmmount,
                    payment = paymentType,
                    billTo = billingAddress,
                    shipTo = billingAddress,
                    lineItems = lineItems,
                };

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the controller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                authorizeNetResponse.Response = controller.GetApiResponse();

                // validate response
                if (authorizeNetResponse.Response != null)
                {
                    if (authorizeNetResponse.Response.messages.resultCode == messageTypeEnum.Ok)
                    {
                        if (authorizeNetResponse.Response.transactionResponse.messages != null)
                        {
                            authorizeNetResponse.Success = true;
                            authorizeNetResponse.Message = string.Format("Transaction Successfull.{0}Transaction ID is {1}", Environment.NewLine, authorizeNetResponse.Response.transactionResponse.transId);
                            authorizeNetResponse.Response = authorizeNetResponse.Response;
                        }
                        else
                        {
                            authorizeNetResponse.Success = false;
                            authorizeNetResponse.Message = string.Format("Transaction Failed.{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, authorizeNetResponse.Response.transactionResponse.errors.Select(x => string.Format("Error: {0}~{1}", x.errorCode, x.errorText)).ToList()));
                            authorizeNetResponse.Response = authorizeNetResponse.Response;
                        }
                    }
                    else
                    {
                        authorizeNetResponse.Success = false;

                        if (authorizeNetResponse.Response.transactionResponse != null && authorizeNetResponse.Response.transactionResponse.errors != null)
                        {
                            authorizeNetResponse.Message = string.Format("Transaction Failed.{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, authorizeNetResponse.Response.transactionResponse.errors.Select(x => string.Format("Error: {0}~{1}", x.errorCode, x.errorText)).ToList()));
                        }
                        else
                        {
                            authorizeNetResponse.Message = string.Format("Transaction Failed.{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, authorizeNetResponse.Response.messages.message.Select(x => string.Format("Error: {0}~{1}", x.code, x.text)).ToList()));
                        }

                        authorizeNetResponse.Response = authorizeNetResponse.Response;
                    }
                }
                else
                {
                    authorizeNetResponse.Success = false;
                    authorizeNetResponse.Message = "No valid response from Authorize.Net.";
                    authorizeNetResponse.Response = authorizeNetResponse.Response;
                }
            }
            catch (Exception ex)
            {
                authorizeNetResponse.Success = false;
                authorizeNetResponse.Message = string.Format("Error occured on server. {0}", ex.Message);
            }

            return authorizeNetResponse;
        }

    }

    public class AuthorizeNetResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public createTransactionResponse Response { get; set; }
    }

    public class AuthorizeNetCreditCardModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Card Holder Name can only be 100 characters at max.")]
        [Display(Name = "Card Holder Name")]
        public string CCName { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [CreditCard(ErrorMessage = "Card Number is not valid credit card number.")]
        public string CCCardNumber { get; set; }

        [Required]
        [Display(Name = "Expiry Month")]
        [Range(1, 12)]
        public short CCExpiryMonth { get; set; }

        [Required]
        [Display(Name = "Expiry Year")]
        public int CCExpiryYear { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "CVC must be 3 characters.")]
        [Display(Name = "CVC")]
        public string CCCVC { get; set; }
    }
}