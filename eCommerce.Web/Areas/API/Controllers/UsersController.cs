using eCommerce.Entities;
using eCommerce.Entities.APIEntities;
using eCommerce.Services;
using eCommerce.Shared.Commons;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Helpers;
using eCommerce.Web.Areas.API.Commons.ActionResults;
using eCommerce.Web.Areas.API.Commons.Extensions;
using eCommerce.Web.Areas.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eCommerce.Web.Areas.API.Controllers
{
    [Authorize]
    public class UsersController : APIBaseController
    {
        #region vars
        private const string LocalLoginProvider = "Local";
        private eCommerceUserManager _userManager;
        private eCommerceRoleManager _roleManager;
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
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        #endregion

        #region ctor
        public UsersController()
        {

        }

        public UsersController(eCommerceUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterUserModel model)
        {
            string errorMessage;

            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var user = new eCommerceUser { FullName = model.FullName, UserName = model.UserName, Email = model.Email, RegisteredOn = DateTime.Now };

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (await RoleManager.RoleExistsAsync("User"))
                        {
                            //assign User role to newly registered user
                            await UserManager.AddToRoleAsync(user.Id, "User");
                        }

                        await UserManager.SendEmailAsync(user.Id, EmailTextHelpers.AccountRegisterEmailSubject(AppDataHelper.CurrentLanguage.ID), EmailTextHelpers.AccountRegisterEmailBody(AppDataHelper.CurrentLanguage.ID, this.Url.Link("Login", null)));

                        return APIResult(HttpStatusCode.OK);
                    }
                    else
                    {
                        errorMessage = string.Join("\n", result.Errors);
                    }
                }
                else
                {
                    var errors = new List<string>();
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }

                    errorMessage = string.Join("\n", errors);
                }
            }
            else
            {
                errorMessage = "Please enter valid user data.";
            }

            return APIResult(HttpStatusCode.BadRequest, errorMessage);
        }

        [HttpGet]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            return APIResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public IHttpActionResult UserInfo()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null) return APIResult(HttpStatusCode.NotFound);

            var userInfoModel = new UserInfo()
            {
                ID = user.Id,
                PictureURL = string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, user.Picture != null ? user.Picture.URL : eCommerceConstants.USER_DEFAULT_PICTURE),
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                ZipCode = user.ZipCode,
                RegisteredOn = user.RegisteredOn
            };

            return APIResult(HttpStatusCode.OK, userInfoModel);
        }

        [HttpPost]
        public async Task<APIResult> UpdateUserInfo(UserInfo model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null) return APIResult(HttpStatusCode.NotFound);

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Country = model.Country;
            user.City = model.City;
            user.Address = model.Address;
            user.ZipCode = model.ZipCode;

            var result = await UserManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return APIResult(HttpStatusCode.OK);
            }
            else
            {
                return APIResult(HttpStatusCode.BadRequest, string.Join("\n", result.Errors));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserPassword(UpdatePasswordModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null) return APIResult(HttpStatusCode.NotFound);

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return APIResult(HttpStatusCode.OK);
            }
            else
            {
                return APIResult(HttpStatusCode.BadRequest, string.Join("\n", result.Errors));
            }
        }

        [HttpGet]
        public IHttpActionResult UserComments(string searchTerm = "", int? pageNo = 1, int? recordSize = (int)RecordSizeEnums.Size10)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null)
            {
                return APIResult(HttpStatusCode.NotFound);
            }

            recordSize = recordSize > 0 ? recordSize : (int)RecordSizeEnums.Size10;

            var comments = CommentsService.Instance.SearchComments(entityID: (int)EntityEnums.Product, recordID: null, userID: user.Id, searchTerm: searchTerm, pageNo: pageNo, recordSize: recordSize.Value, count: out int commentsCount);

            var model = new CommentsListModel();
            model.CommentsListFilters.PageNo = pageNo.Value;
            model.CommentsListFilters.RecordSize = recordSize.Value;

            if (comments != null && comments.Count > 0)
            {
                var products = ProductsService.Instance.GetProductsByIDs(comments.Select(x=>x.RecordID).ToList());

                model.TotalComments = commentsCount;
                model.TotalPages = commentsCount / recordSize.Value;

                foreach (var comment in comments)
                {
                    var product = products.FirstOrDefault(x => x.ID == comment.RecordID);

                    if (product != null)
                    {
                        var currentLanguageProductRecord = product.ProductRecords.FirstOrDefault(x => x.LanguageID == AppDataHelper.CurrentLanguage.ID);

                        model.Comments.Add(new ProductCommentEntity()
                        {
                            ID = comment.ID,
                            Product = new CommentProduct()
                            {
                                ProductID = product.ID,
                                ProductTitle = currentLanguageProductRecord != null ? currentLanguageProductRecord.Name : string.Empty
                            },
                            TimeStamp = comment.TimeStamp,
                            UserID = comment.UserID,
                            UserName = comment.User != null ? comment.User.UserName : string.Empty,
                            Text = comment.Text,
                            Rating = comment.Rating
                        });
                    }
                }
            }

            return APIResult(HttpStatusCode.OK, model);
        }

        [HttpGet]
        public IHttpActionResult UserOrders(int? orderStatus = 0, int? pageNo = 1, int? recordSize = (int)RecordSizeEnums.Size10)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null)
            {
                return APIResult(HttpStatusCode.NotFound);
            }

            recordSize = recordSize > 0 ? recordSize : (int)RecordSizeEnums.Size10;

            var userOrders = OrdersService.Instance.SearchOrders(user.Id, null, orderStatus, pageNo, recordSize.Value, count: out int ordersCount);

            var model = new OrdersListModel();
            model.OrdersListFilters.PageNo = pageNo.Value;
            model.OrdersListFilters.RecordSize = recordSize.Value;

            if (userOrders != null && userOrders.Count > 0)
            {
                foreach (var userOrder in userOrders)
                {
                    var status = userOrder.OrderHistory != null && userOrder.OrderHistory.Count > 0 ? userOrder.OrderHistory.OrderByDescending(x => x.ModifiedOn).FirstOrDefault() : new OrderHistory();

                    model.Orders.Add(new UserOrder()
                    {
                        ID = userOrder.ID,
                        FinalAmmount = userOrder.FinalAmmount,
                        PlacedOn = userOrder.PlacedOn,
                        Status = string.Format("PP.Tracking.OrderStatus.{0}", ((OrderStatus)status.OrderStatus).ToString()).LocalizedString(),
                        PaymentMethod = string.Format("PP.Tracking.PaymentMethods.{0}{1}", ((PaymentMethods)userOrder.PaymentMethod).ToString(), string.Format("{0}", !string.IsNullOrEmpty(userOrder.TransactionID) ? "PP.Tracking.TransactionID".LocalizedString() + ": " + userOrder.TransactionID : string.Empty)).LocalizedString()
                    });
                }
            }

            return APIResult(HttpStatusCode.OK, model);
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserAvatar()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return APIResult(HttpStatusCode.NotFound);
            }

            var pictures = HttpContext.Current.Request.Files;

            if (pictures.Count > 0)
            {
                var picture = pictures[0];

                var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);

                var path = HttpContext.Current.Server.MapPath(string.Format("~{0}", eCommerceConstants.MAIN_IMAGES_DIRECTORY)) + fileName;

                picture.SaveAs(path);

                var dbPicture = new Picture
                {
                    URL = fileName,
                    ModifiedOn = DateTime.Now
                };

                int pictureID = SharedService.Instance.SavePicture(dbPicture);

                user.PictureID = pictureID;

                var result = await UserManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    return APIResult(HttpStatusCode.OK);
                }
                else
                {
                    return APIResult(HttpStatusCode.BadRequest, new { Errors = string.Join("\n", result.Errors) });
                }
            }
            else
            {
                return APIResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
