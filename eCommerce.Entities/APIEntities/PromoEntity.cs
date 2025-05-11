using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.APIEntities
{
    public class PromoEntity
    {
        public string Code { get; set; }
        public string PromoType { get; set; }
        public decimal Value { get; set; }
        public Nullable<DateTime> ValidTill { get; set; }
    }
}
