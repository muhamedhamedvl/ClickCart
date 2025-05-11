using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.APIEntities
{
    public class ProductEntity
    {
        public ProductEntity()
        {
            ProductSpecifications = new List<ProductSpecificationEntity>();
            Pictures = new List<PictureEntity>();
        }

        public int ID { get; set; }
        public int CategoryID { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Cost { get; set; }
        public bool IsFeatured { get; set; }

        public string SKU { get; set; }
        public string Tags { get; set; }
        public string Barcode { get; set; }
        public string Supplier { get; set; }

        public int StockQuantity { get; set; }

        public int TotalRatings { get; set; }
        public int AverageRatings { get; set; }

        public virtual List<ProductSpecificationEntity> ProductSpecifications { get; set; }
        public virtual List<PictureEntity> Pictures { get; set; }
    }

    public class ProductSpecificationEntity
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
