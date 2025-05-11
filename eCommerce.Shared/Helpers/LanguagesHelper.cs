using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Shared.Helpers
{
    public static class LanguagesHelper
    {
        /// <summary>
        /// All languages that are marked true for isEnabled property in database.
        /// </summary>
        public static List<Language> EnabledLanguages { get; set; }

        /// <summary>
        /// All languages that are marked true for isEnabled property in database.
        /// </summary>
        public static List<Language> LanguagesWithResources {
            get
            {
                return EnabledLanguages != null && EnabledLanguages.Count > 0 ? EnabledLanguages.Where(x => LanguageIDsWithResources.Contains(x.ID)).ToList() : null;
            }
        }
        private static List<int> LanguageIDsWithResources { get; set; }

        /// <summary>
        /// A language that is marked true for IsDefault property in database.
        /// </summary>
        public static Language DefaultLanguage { get; set; }

        /// <summary>
        /// English Langauge Shortcode used for product detail title sanitization
        /// </summary>
        public static string EnglishLanguageShortCode = "en";

        public static void LoadLanguages(List<Language> enabledLanguages, Language defaultLanguage, List<int> languageIDsWithResources)
        {
            EnabledLanguages = enabledLanguages;

            DefaultLanguage = defaultLanguage;

            LanguageIDsWithResources = languageIDsWithResources;
        }
    }
}
