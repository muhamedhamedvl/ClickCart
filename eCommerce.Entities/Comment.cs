using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Comment : BaseEntity
    {
        public DateTime TimeStamp { get; set; }

        public string UserID { get; set; }
        public virtual eCommerceUser User { get; set; }

        public int Rating { get; set; }
        public string Text { get; set; }
        
        public int EntityID { get; set; }
        public int RecordID { get; set; }

        public int LanguageID { get; set; }
    }
}
