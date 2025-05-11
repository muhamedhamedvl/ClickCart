using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace eCommerce.Shared.Attributes
{
    public sealed class PageDetailAttribute : OutputProcessorActionFilterAttribute
    {
        protected override string Process(string data)
        {
            if (data.Contains("pp-widgets"))
            {
                return data.TextRegulator();
            }
            else if (data.Contains("dp-widgets"))
            {
                return data.TextRegulator(isDP: true);
            }
            else return data;
        }
    }
}
