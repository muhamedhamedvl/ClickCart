using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class NewsletterSubscription : BaseEntity
    {
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public string UserID { get; set; }
    }
}
