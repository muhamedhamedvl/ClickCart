using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class ConfigurationsService
    {
        #region Define as Singleton
        private static ConfigurationsService _Instance;

        public static ConfigurationsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ConfigurationsService();
                }

                return (_Instance);
            }
        }

        private ConfigurationsService()
        {
        }
        #endregion

        public List<Configuration> GetAllConfigurations()
        {
            var context = DataContextHelper.GetNewContext();

            return context.Configurations.ToList();
        }

        public List<Configuration> GetConfigurationsByType(int configurationType)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Configurations.Where(x => x.ConfigurationType == configurationType).ToList();
        }

        public Configuration GetConfigurationByKey(string key)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Configurations.FirstOrDefault(x => x.Key == key);
        }

        public bool UpdateConfiguration(Configuration configuration)
        {
            var context = DataContextHelper.GetNewContext();

            context.Entry(configuration).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public bool UpdateConfigurationValue(string key, string value)
        {
            var context = DataContextHelper.GetNewContext();

            var configuration = context.Configurations.Find(key);

            configuration.Value = value;

            context.Entry(configuration).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public List<Configuration> SearchConfigurations(int? configurationType, string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var configurations = context.Configurations.AsQueryable();

            if (configurationType.HasValue && configurationType.Value > 0)
            {
                configurations = configurations.Where(x => x.ConfigurationType == configurationType.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                configurations = configurations.Where(x => x.Key.ToLower().Contains(searchTerm.ToLower()));
            }

            count = configurations.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return configurations.OrderBy(x => x.Key).Skip(skipCount).Take(recordSize).ToList();
        }
    }
}
