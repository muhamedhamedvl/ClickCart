using eCommerce.Entities;
using eCommerce.Entities.APIEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.Areas.API.Models
{
    public class StoreInfoModel
    {
        public string StoreName { get; set; }
        public string StoreIntro { get; set; }
        public string Address { get; set; }
        public string CurrencySymbol { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string WhatsAppNumber { get; set; }
        public string Email { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string InstagramURL { get; set; }
        public string YouTubeURL { get; set; }
        public string LinkedInURL { get; set; }
    }

    public class Slider
    {
        public string ImageLink { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }

    public class MenuCategory : CategoryEntity
    {
        public List<MenuCategory> Children { get; set; }
    }

    public class CommentsListModel
    {
        public CommentsListModel()
        {
            CommentsListFilters = new CommentsListFilters();
            Comments = new List<ProductCommentEntity>();
        }

        public int TotalComments { get; set; }
        public int TotalPages { get; set; }
        public CommentsListFilters CommentsListFilters { get; set; }

        public List<ProductCommentEntity> Comments { get; set; }
    }

    public class CommentsListFilters
    {
        public int PageNo { get; set; }
        public int RecordSize { get; set; }
    }
}