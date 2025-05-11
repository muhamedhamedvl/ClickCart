using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public static class DataContextHelper
    {
        public static eCommerceContext GetNewContext()
        {
            return new eCommerceContext();
        }
    }
}
