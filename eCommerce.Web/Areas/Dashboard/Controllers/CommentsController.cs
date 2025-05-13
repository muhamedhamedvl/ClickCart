using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;
using eCommerce.Web.ViewModels;
using System.Threading;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class CommentsController : DashboardBaseController
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

        public async Task<ActionResult> Index(string userID, string searchTerm, int? pageNo, int entityID = (int)EntityEnums.Product, bool showUserCommentsOnly = false)
        {
            CommentsListingViewModel model = new CommentsListingViewModel
            {
                SearchTerm = searchTerm
            };

            if (!string.IsNullOrEmpty(userID))
            {
                model.User = await UserManager.FindByIdAsync(userID);
            }

            model.Comments = CommentsService.Instance.SearchComments(entityID: entityID, recordID: null, userID: userID, searchTerm: model.SearchTerm, pageNo: pageNo, recordSize: (int)RecordSizeEnums.Size10, count: out int commentsCount);

            if (model.Comments != null && model.Comments.Count > 0)
            {
                var productIDs = model.Comments.Select(x => x.RecordID).ToList();

                model.CommentedProducts = ProductsService.Instance.GetProductsByIDs(productIDs);
            }

            model.Pager = new Pager(commentsCount, pageNo, (int)RecordSizeEnums.Size10);

            if (showUserCommentsOnly)
            {
                return PartialView("_UserComments", model);
            }
            else return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int ID)
        {
            try
            {
                var comment = CommentsService.Instance.GetCommentByID(ID);

                if (comment == null)
                {
                    return Json(new { Success = false, Message = "Comment not found." });
                }

                var currentUserId = User.Identity.GetUserId();

                if (!User.Identity.IsAuthenticated || (!User.IsInRole("Administrator") && comment.UserID.ToString() != currentUserId))
                {
                    return Json(new { Success = false, Message = "Not authorized." });
                }

                bool success = CommentsService.Instance.DeleteComment(comment);
                return Json(new
                {
                    Success = success,
                    Message = success ? string.Empty : "Failed to delete comment."
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "An error occurred." });
            }
        }
    }
}