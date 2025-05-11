using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class LanguagesController : DashboardBaseController
    {
        public ActionResult Index(string searchTerm, int? pageNo)
        {
            var recordSize = (int)RecordSizeEnums.Size10;

            LanguagesListingViewModel model = new LanguagesListingViewModel
            {
                SearchTerm = searchTerm
            };

            model.Languages = LanguagesService.Instance.SearchLanguages(searchTerm, out int count, pageNo: pageNo, recordSize: recordSize);

            model.LanguageIDsWithResources = LanguagesService.Instance.LanguagesWithResources();

            model.Pager = new Pager(count, pageNo, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            LanguageActionViewModel model = new LanguageActionViewModel();

            if (ID.HasValue)
            {
                var language = LanguagesService.Instance.GetLanguageByID(ID.Value);

                if (language == null) return HttpNotFound();

                model.ID = language.ID;
                model.Name = language.Name;
                model.ShortCode = language.ShortCode;
                model.IsEnabled = language.IsEnabled;
                model.IsDefault = language.IsDefault;
                model.IsRTL = language.IsRTL;
                model.IconCode = language.IconCode;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Action(LanguageActionViewModel model)
        {
            JsonResult json = new JsonResult();

            try
            {
                if (model.ID > 0)
                {
                    var language = LanguagesService.Instance.GetLanguageByID(model.ID);

                    if (language == null)
                    {
                        throw new Exception("Dashboard.Languages.Action.Validation.LanguageNotFound".LocalizedString());
                    }

                    language.ID = model.ID;
                    language.Name = model.Name;
                    language.ShortCode = model.ShortCode;
                    language.IsEnabled = model.IsEnabled;

                    if(language.IsDefault && !model.IsDefault)
                    {
                        throw new Exception("Dashboard.Languages.Action.Validation.DefaultLanguageIsMust".LocalizedString());
                    }

                    var makeDefault = false;
                    if (model.IsDefault)
                    {
                        if(!language.IsDefault)
                        {
                            language.IsDefault = true;
                            makeDefault = true;
                        }
                    }
                    else
                    {
                        language.IsDefault = false;
                    }

                    language.IsRTL = model.IsRTL;
                    language.IconCode = model.IconCode;

                    if (!LanguagesService.Instance.UpdateLanguage(language, makeDefault))
                    {
                        throw new Exception("Dashboard.Languages.Action.Validation.UnableToUpdateLanguage".LocalizedString());
                    }
                }
                else
                {
                    var language = new Language
                    {
                        Name = model.Name,
                        ShortCode = model.ShortCode,
                        IsEnabled = model.IsEnabled,
                        IsDefault = model.IsDefault,
                        IsRTL = model.IsRTL,
                        IconCode = model.IconCode
                    };

                    var makeDefault = false;
                    if (model.IsDefault)
                    {
                        language.IsDefault = true;
                        makeDefault = true;
                    }
                    else
                    {
                        language.IsDefault = false;
                    }


                    if (!LanguagesService.Instance.AddLanguage(language, makeDefault))
                    {
                        throw new Exception("Dashboard.Languages.Action.Validation.UnableToCreateLanguage".LocalizedString());
                    }
                }

                UpdateLanguages();

                json.Data = new { Success = true };
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }
            
            return json;
        }

        [HttpPost]
        public JsonResult ChangeLanguageStatus(int ID, bool disable = true)
        {
            JsonResult json = new JsonResult();

            try
            {
                var language = LanguagesService.Instance.GetLanguageByID(ID);

                if (language == null)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.LanguageNotFound".LocalizedString());
                }

                if(disable)
                {
                    if (language.IsDefault)
                    {
                        throw new Exception("Dashboard.Languages.Action.Validation.DefaultLanguageCantBeDisabled".LocalizedString());
                    }

                    language.IsEnabled = false;
                }
                else language.IsEnabled = true;

                if (!LanguagesService.Instance.UpdateLanguage(language))
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.UnableToUpdateLanguage".LocalizedString());
                }

                UpdateLanguages();

                json.Data = new { Success = true };
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }

        [HttpGet]
        public ActionResult Resources(int? ID)
        {
            LanguageResourceViewModel model = new LanguageResourceViewModel();

            if (ID.HasValue)
            {
                model.ID = ID.Value;
                model.Language = LanguagesService.Instance.GetLanguageByID(ID.Value);

                model.HasResources = LanguagesService.Instance.LanguageHasResources(model.Language);
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ExportResources(int? ID)
        {
            Language language = null;

            if(ID.HasValue)
            {
                language = LanguagesService.Instance.GetLanguageByID(ID.Value);
            }

            if (language == null) return HttpNotFound();

            var list = LanguagesService.Instance.GetLanguageResources(language.ID);

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Language");

                    foreach (var resource in list)
                    {
                        xmlWriter.WriteStartElement("Resource");
                        xmlWriter.WriteAttributeString("Key", resource.Key.Replace(ID.Value + "_", ""));
                        xmlWriter.WriteAttributeString("Value", resource.Value);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }

                return File(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(stream.ToArray())), "application/xml", "language_pack.xml");
            }
        }

        [HttpPost]
        public JsonResult ImportResources(int? ID)
        {
            var json = new JsonResult();

            try
            {
                if (Request.Files.Count <= 0)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.SelectFileToUpload".LocalizedString());
                }

                Language language = null;
                
                if(ID.HasValue)
                {
                    language = LanguagesService.Instance.GetLanguageByID(ID.Value);
                }

                if(language == null)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.LanguageNotFound".LocalizedString());
                }

                var file = Request.Files[0];

                var resources = new List<LanguageResource>();
                
                try
                {
                    var document = new XmlDocument();
                    document.Load(file.InputStream);

                    var languageNode = document.ChildNodes[1];
                    
                    foreach (XmlNode resourceNode in languageNode.ChildNodes)
                    {
                        if(resourceNode.Attributes.Count > 0 && resourceNode.Attributes["Key"] != null && resourceNode.Attributes["Value"] != null)
                        {
                            resources.Add(new LanguageResource() { Key = string.Format(language.ID + "_" + resourceNode.Attributes["Key"].Value), Value = resourceNode.Attributes["Value"].Value, LanguageID = language.ID });
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.SelectValidFileToUpload".LocalizedString());
                }

                if(resources.Count <= 0)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.NoResourcesInFile".LocalizedString());
                }

                var result = LanguagesService.Instance.ImportLanguageResources(language.ID, resources);

                if (!result)
                {
                    throw new Exception("Dashboard.Languages.Action.Validation.UnableToImportResources".LocalizedString());
                }

                UpdateLanguages();
                LocalizationHelper.LoadResourceLocalizations(LanguagesService.Instance.GetAllLanguageResources());

                json.Data = new { Success = true };
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }

        private void UpdateLanguages()
        {
            var languageIDsWithResources = LanguagesService.Instance.LanguagesWithResources();
            var enabledLanguages = LanguagesService.Instance.GetLanguages(enabledLanguagesOnly: true, resourceLanguagesOnly: false);
            LanguagesHelper.LoadLanguages(enabledLanguages: enabledLanguages, defaultLanguage: enabledLanguages.FirstOrDefault(x => x.IsDefault), languageIDsWithResources: languageIDsWithResources);
        }
    }
}