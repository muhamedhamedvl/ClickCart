using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class LanguagesService
    {
        #region Define as Singleton
        private static LanguagesService _Instance;

        public static LanguagesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LanguagesService();
                }

                return (_Instance);
            }
        }

        private LanguagesService()
        {
        }
        #endregion

        public List<Language> GetLanguages(bool enabledLanguagesOnly = false, bool resourceLanguagesOnly = true, int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var languages = context.Languages
                                    .Where(x => !x.IsDeleted)
                                    .AsQueryable();

            if(enabledLanguagesOnly)
            {
                languages = languages.Where(x => x.IsEnabled);
            }

            if(resourceLanguagesOnly)
            {
                var languageIDsWithResources = context.LanguageResources.Select(x => x.LanguageID).Distinct();

                languages = languages.Where(x=>languageIDsWithResources.Contains(x.ID));
            }

            languages = languages.OrderBy(x => x.Name);

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                languages = languages.Skip(skip)
                                       .Take(recordSize.Value);
            }

            return languages.ToList();
        }

        public List<Language> SearchLanguages(string searchTerm, out int count, bool enabledLanguagesOnly = false, int? pageNo = 1, int? recordSize = 0)
        {
            var context = DataContextHelper.GetNewContext();

            var languages = context.Languages
                                    .Where(x => !x.IsDeleted)
                                    .AsQueryable();

            if(!string.IsNullOrEmpty(searchTerm))
            {
                languages = languages.Where(x => x.Name.Contains(searchTerm));
            }

            if (enabledLanguagesOnly)
            {
                languages = languages.Where(x => x.IsEnabled);
            }

            count = languages.Count();

            languages = languages.OrderBy(x => x.ID);

            if (recordSize.HasValue && recordSize.Value > 0)
            {
                pageNo = pageNo ?? 1;
                var skip = (pageNo.Value - 1) * recordSize.Value;

                languages = languages.Skip(skip)
                                       .Take(recordSize.Value);
            }

            return languages.ToList();
        }

        public Language GetDefaultLanguage()
        {
            var context = DataContextHelper.GetNewContext();

            return context.Languages.FirstOrDefault(x => x.IsDefault && !x.IsDeleted);
        }

        public Language GetLanguageByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Languages.FirstOrDefault(x => x.ID == ID && !x.IsDeleted);
        }

        public Language GetLanguageByShortCode(string shortCode)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Languages.FirstOrDefault(x => x.ShortCode == shortCode && !x.IsDeleted);
        }

        public bool AddLanguage(Language language, bool makeDefault = false)
        {
            var context = DataContextHelper.GetNewContext();

            if (makeDefault)
            {
                var existingDefaultLanguages = context.Languages.Where(x => x.IsDefault).ToList();

                existingDefaultLanguages.ForEach(x => x.IsDefault = false);
            }

            context.Languages.Add(language);

            return context.SaveChanges() > 0;
        }

        public bool UpdateLanguage(Language language, bool makeDefault = false)
        {
            var context = DataContextHelper.GetNewContext();

            if (makeDefault)
            {
                var existingDefaultLanguages = context.Languages.Where(x => x.IsDefault).ToList();

                existingDefaultLanguages.ForEach(x => x.IsDefault = false);
            }

            context.Entry(language).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public bool LanguageHasResources(Language language)
        {
            var context = DataContextHelper.GetNewContext();

            var languageResource = context.LanguageResources.FirstOrDefault(x => x.LanguageID == language.ID);

            return languageResource != null;
        }

        public List<int> LanguagesWithResources()
        {
            var context = DataContextHelper.GetNewContext();

            var languageIDs = context.LanguageResources.Select(x => x.LanguageID).Distinct().ToList();

            return languageIDs;
        }

        public List<LanguageResource> GetAllLanguageResources()
        {
            var context = DataContextHelper.GetNewContext();

            return context.LanguageResources.ToList();
        }

        public List<LanguageResource> GetLanguageResources(int languageID)
        {
            var context = DataContextHelper.GetNewContext();

            return context.LanguageResources.Where(x => x.LanguageID == languageID).ToList();
        }

        public List<LanguageResource> GetLanguagesResources(List<int> languageIDs)
        {
            var context = DataContextHelper.GetNewContext();

            return context.LanguageResources.Where(x => languageIDs.Contains(x.LanguageID)).ToList();
        }

        public bool ImportLanguageResources(int languageID, List<LanguageResource> resources)
        {
            var context = DataContextHelper.GetNewContext();

            var existingResources = context.LanguageResources.Where(x=>x.LanguageID == languageID);

            if(existingResources.Count() > 0)
            {
                context.LanguageResources.RemoveRange(existingResources);
            }

            context.LanguageResources.AddRange(resources);

            return context.SaveChanges() > 0;
        }
    }
}
