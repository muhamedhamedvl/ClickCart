using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class CommentsService
    {
        #region Define as Singleton
        private static CommentsService _Instance;

        public static CommentsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CommentsService();
                }

                return (_Instance);
            }
        }

        private CommentsService()
        {
        }
        #endregion
        
        public bool AddComment(Comment comment)
        {
            var context = DataContextHelper.GetNewContext();

            context.Comments.Add(comment);

            return context.SaveChanges() > 0;
        }

        public List<Comment> SearchComments(int? entityID, int? recordID, string userID, string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var comments = context.Comments
                                  .Where(x => !x.IsDeleted)
                                  .AsQueryable();

            if (entityID.HasValue && entityID.Value > 0)
            {
                comments = comments.Where(x => x.EntityID == entityID.Value);
            }

            if (recordID.HasValue && recordID.Value > 0)
            {
                comments = comments.Where(x => x.RecordID == recordID.Value);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                comments = comments.Where(x => x.UserID == userID);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                comments = comments.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower()));
            }

            count = comments.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return comments.Include("User")
                           .Include("User.Picture")
                           .OrderByDescending(x => x.TimeStamp)
                           .Skip(skipCount)
                           .Take(recordSize)
                           .ToList();
        }
        
        public Comment GetCommentByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var comment = context.Comments.FirstOrDefault(x => x.ID == ID);

            return comment != null && !comment.IsDeleted ? comment : null;
        }

        public ProductRating GetProductRating(int productID)
        {
            var context = DataContextHelper.GetNewContext();

            var productRating = new ProductRating();

            var productComments = context.Comments.Where(x => !x.IsDeleted && x.EntityID == (int)EntityEnums.Product && x.RecordID == productID);

            productRating.TotalRatings = productComments.Count();
            productRating.AverageRating = productRating.TotalRatings > 0 ? (int)productComments.Average(x => x.Rating) : 0;

            return productRating;
        }

        public bool DeleteComment(Comment comment)
        {
            var context = DataContextHelper.GetNewContext();

            //var comment = context.Comments.Find(ID);

            comment.IsDeleted = true;

            context.Entry(comment).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }
    }
}
