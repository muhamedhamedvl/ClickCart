using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public bool IsEnabled { get; set; }
        public string ResourceFileName { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDefault { get; set; }

        public string IconCode { get; set; }
    }
}
