using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class LanguageResource : BaseEntity
    {
        public string Key { get; set; }
        public int LanguageID { get; set; }
        public string Value { get; set; }
    }
}
