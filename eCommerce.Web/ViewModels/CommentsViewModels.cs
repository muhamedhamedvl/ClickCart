using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.ViewModels
{
    public class CommentViewModel
    {
        public string Text { get; set; }

        public int Rating { get; set; }

        public int EntityID { get; set; }

        public int RecordID { get; set; }
    }

    public class EntityCommentsViewModel : PageViewModel
    {
        public List<Comment> Comments { get; set; }
        public int EntityID { get; set; }
        public int RecordID { get; set; }
    }
    public class CommentsListingViewModel : PageViewModel
    {
        public string SearchTerm { get; set; }
        public eCommerceUser User { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Product> CommentedProducts { get; set; }
        public Pager Pager { get; set; }
    }
}