using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.APIEntities
{
    public class CategoryEntity
    {
        public int ID { get; set; }
        public int? ParentCategoryID { get; set; }
        
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public bool IsFeatured { get; set; }
        public string SanitizedName { get; set; }

        public int DisplaySeqNo { get; set; }

        public string Picture { get; set; }

        public int ProductsCount { get; set; }
    }
}
