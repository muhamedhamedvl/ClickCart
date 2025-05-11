using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class SharedService
    {
        #region Define as Singleton
        private static SharedService _Instance;

        public static SharedService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SharedService();
                }

                return (_Instance);
            }
        }

        private SharedService()
        {
        }
        #endregion
        
        public int SavePicture(Picture picture)
        {
            var context = DataContextHelper.GetNewContext();

            context.Pictures.Add(picture);

            context.SaveChanges();

            return picture.ID;
        }

        public bool SaveNewsletterSubscription(NewsletterSubscription newsletterSubscription)
        {
            var context = DataContextHelper.GetNewContext();

            //check for an existing subscription.
            var existingSubscription = context.NewsletterSubscriptions.FirstOrDefault(x => x.EmailAddress == newsletterSubscription.EmailAddress);

            if(existingSubscription == null)
            {
                context.NewsletterSubscriptions.Add(newsletterSubscription);
            }

            return context.SaveChanges() > 0;
        }


        public List<NewsletterSubscription> SearchNewsletterSubscription(string searchTerm, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var newsletterSubscriptions = context.NewsletterSubscriptions.Where(x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                newsletterSubscriptions = newsletterSubscriptions.Where(x => x.EmailAddress.Contains(searchTerm));
            }

            count = newsletterSubscriptions.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return newsletterSubscriptions.OrderBy(x => x.ID).Skip(skipCount).Take(recordSize).ToList();
        }
    }
}
