using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Controllers
{
    public class CommentsController : PublicBaseController
    {
        private eCommerceUserManager _userManager;
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

        [HttpPost]
        public JsonResult LeaveComment(CommentViewModel model)
        {
            JsonResult result = new JsonResult();

            try
            {
                var comment = new Comment
                {
                    Text = model.Text,
                    Rating = model.Rating,
                    EntityID = model.EntityID,
                    RecordID = model.RecordID,
                    UserID = User.Identity.GetUserId(),
                    TimeStamp = DateTime.Now,

                    LanguageID = AppDataHelper.CurrentLanguage.ID
                };

                var res = CommentsService.Instance.AddComment(comment);

                result.Data = new { Success = res };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
            }

            return result;
        }
        
        public ActionResult ProductComments(int productID, int pageNo = 1, int recordSize = 10)
        {
            EntityCommentsViewModel model = new EntityCommentsViewModel
            {
                EntityID = (int)EntityEnums.Product,
                RecordID = productID
            };

            model.Comments = CommentsService.Instance.SearchComments(entityID: model.EntityID, recordID: model.RecordID, userID: null, searchTerm: null, pageNo: pageNo, recordSize: recordSize, out int count);

            return PartialView("_ProductComments", model);
        }

        public async Task<ActionResult> UserComments(string userID, string searchTerm, int? pageNo = 1, int entityID = (int)EntityEnums.Product, bool isPartial = false)
        {
            CommentsListingViewModel model = new CommentsListingViewModel
            {
                SearchTerm = searchTerm
            };

            if (!string.IsNullOrEmpty(userID))
            {
                model.User = await UserManager.FindByIdAsync(userID);
            }
            else
            {
                model.User = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            }

            model.Comments = CommentsService.Instance.SearchComments(entityID: entityID, recordID: null, userID: model.User.Id, searchTerm: model.SearchTerm, pageNo: pageNo, recordSize: (int)RecordSizeEnums.Size10, count: out int commentsCount);
            
            if (model.Comments != null && model.Comments.Count > 0)
            {
                var productIDs = model.Comments.Select(x => x.RecordID).ToList();

                model.CommentedProducts = ProductsService.Instance.GetProductsByIDs(productIDs);
            }

            model.Pager = new Pager(commentsCount, pageNo, (int)RecordSizeEnums.Size10);

            if (isPartial)
            {
                return PartialView("_UserCommentsListing", model);
            }
            else
            {
                return PartialView("_UserComments", model);
            }
        }

        [HttpPost]
        public JsonResult DeleteComment(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var comment = CommentsService.Instance.GetCommentByID(ID);

                if (comment != null && User.Identity.IsAuthenticated && (User.IsInRole("Administrator") || comment.UserID == User.Identity.GetUserId()))
                {
                    var operation = CommentsService.Instance.DeleteComment(comment);

                    result.Data = new { Success = operation, Message = operation ? string.Empty : "PP.ProductDetails.Comments.Validations.UnableToDeleteComment".LocalizedString() };
                }
                else
                {
                    throw new Exception("PP.ProductDetails.Comments.Validations.NotAuthorizedToDeleteComment".LocalizedString());
                }
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = string.Format("{0}", ex.Message) };
            }

            return result;
        }
    }
}