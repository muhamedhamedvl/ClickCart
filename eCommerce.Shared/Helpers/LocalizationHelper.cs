using eCommerce.Entities;
using eCommerce.Shared.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eCommerce.Shared.Helpers
{
    public static class LocalizationHelper
    {
        public static ConcurrentDictionary<string, HtmlString> ResourcesDictionary { get; set; }

        public static void LoadResourceLocalizations(List<LanguageResource> list)
        {
            if (list != null && list.Count > 0)
            {
                ResourcesDictionary = new ConcurrentDictionary<string, HtmlString>(list.Distinct(new LanguageResourceComparer()).ToDictionary(x => x.Key.SafeTrim(), x => new HtmlString(x.Value.SafeTrim())));
            }
            else ResourcesDictionary = new ConcurrentDictionary<string, HtmlString>();
        }

        public static HtmlString GetLocalizedString(string resourceKey, int languageID)
        {
            HtmlString htmlString = null;

            ResourcesDictionary.TryGetValue(string.Format("{0}_{1}", languageID, resourceKey), out htmlString);

            if (htmlString == null)
            {
                var key = resourceKey.Contains(".") ? resourceKey.Substring(resourceKey.LastIndexOf('.')).Replace(".", "") : resourceKey;

                htmlString = new HtmlString(key.MakeWord());

                //throw new Exception($"resource missing: {resourceKey}");
            }

            return htmlString;
        }

        public static HtmlString Localized(this string resourceKey, int languageID)
        {
            return GetLocalizedString(resourceKey, languageID);
        }

        public static HtmlString Localized(this string resourceKey)
        {
            return GetLocalizedString(resourceKey, AppDataHelper.CurrentLanguage.ID);
        }

        public static string LocalizedString(this string resourceKey)
        {
            return GetLocalizedString(resourceKey, AppDataHelper.CurrentLanguage.ID).ToString();
        }
    }

    public class LanguageResourceComparer : IEqualityComparer<LanguageResource>
    {
        public bool Equals(LanguageResource x, LanguageResource y)
        {
            // Two items are equal if their keys are equal.
            return x.Key == y.Key;
        }

        public int GetHashCode(LanguageResource obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
