using eCommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class DashboardService
    {
        #region Define as Singleton
        private static DashboardService _Instance;

        public static DashboardService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DashboardService();
                }

                return (_Instance);
            }
        }

        private DashboardService()
        {
        }
        #endregion

        public int GetUserCount()
        {
            var context = DataContextHelper.GetNewContext();

            return context.Users.Count();
        }
        public int GetRolesCount()
        {
            var context = DataContextHelper.GetNewContext();

            return context.Roles.Count();
        }
    }
}
