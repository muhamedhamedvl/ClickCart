using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.APIEntities
{
    public class ProductCommentEntity
    {
        public int ID { get; set; }
        public CommentProduct Product { get; set; }
        public DateTime TimeStamp { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
    }

    public class CommentProduct
    {
        public int ProductID { get; set; }
        public string ProductTitle { get; set; }
    }
}
