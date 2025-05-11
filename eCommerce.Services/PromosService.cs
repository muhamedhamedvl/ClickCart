using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class PromosService
    {
        #region Define as Singleton
        private static PromosService _Instance;

        public static PromosService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PromosService();
                }

                return (_Instance);
            }
        }

        private PromosService()
        {
        }
        #endregion

        public List<Promo> SearchPromos(string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var promos = context.Promos
                                .Where(x => !x.IsDeleted)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                promos = promos.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            count = promos.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return promos.OrderByDescending(x => x.Name).Skip(skipCount).Take(recordSize).ToList();
        }

        public Promo GetPromoByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Promos.FirstOrDefault(x=> !x.IsDeleted && x.ID == ID);
        }
        public Promo GetPromoByCode(string code)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Promos.FirstOrDefault(x => !x.IsDeleted && x.Code == code);
        }

        public bool SavePromo(Promo Promo)
        {
            var context = DataContextHelper.GetNewContext();

            context.Promos.Add(Promo);

            return context.SaveChanges() > 0;
        }

        public bool UpdatePromo(Promo promo)
        {
            var context = DataContextHelper.GetNewContext();

            context.Entry(promo).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }
        
        public bool DeletePromo(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            var promos = context.Promos.Find(ID);

            promos.IsDeleted = true;

            context.Entry(promos).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }
    }
}
