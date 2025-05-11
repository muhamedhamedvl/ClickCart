using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Shared.Helpers
{
    public static class RatingsHelpers
    {
        public static double GetAverageRating(List<int> ratings)
        {
            double average = 0;

            if (ratings != null && ratings.Count > 0)
            {
                average = Math.Ceiling(ratings.Average());
            }

            return average;
        }
    }
}
