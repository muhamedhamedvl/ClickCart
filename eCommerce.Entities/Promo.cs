using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Promo : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
        public int PromoType { get; set; }
        public decimal Value { get; set; }
        public Nullable<DateTime> ValidTill { get; set; }
    }
}
