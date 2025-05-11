using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.APIEntities
{
    public class CartItemEntity
    {
        public CartItemProduct Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ProductTotal
        {
            get
            {
                return Price * Quantity;
            }
        }
    }

    public class CartItemProduct
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
    }
}
