using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Controllers
{
    public class SharedController : PublicBaseController
    {
        [HttpPost]
        public JsonResult UploadPictures()
        {
            JsonResult result = new JsonResult();

            List<object> picturesJSON = new List<object>();

            var pictures = Request.Files;

            for (int i = 0; i < pictures.Count; i++)
            {
                var picture = pictures[i];

                var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);

                var path = Server.MapPath("~/content/images/") + fileName;

                picture.SaveAs(path);

                var dbPicture = new Picture
                {
                    URL = fileName,
                    ModifiedOn = DateTime.Now
                };

                int pictureID = SharedService.Instance.SavePicture(dbPicture);

                picturesJSON.Add(new { ID = pictureID, pictureURL = fileName });
            }

            result.Data = picturesJSON;

            return result;
        }

        [HttpPost]
        public JsonResult UploadPicturesWithoutDatabase(string subFolder, bool isSiteFolder = false)
        {
            JsonResult result = new JsonResult();

            List<object> picturesJSON = new List<object>();

            var pictures = Request.Files;

            for (int i = 0; i < pictures.Count; i++)
            {
                var picture = pictures[i];

                var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);

                var folderPath = string.Format("~/content/images/{0}{1}", isSiteFolder ? "site/" : string.Empty, !string.IsNullOrEmpty(subFolder) ? subFolder + "/" : string.Empty);

                var path = Server.MapPath(folderPath) + fileName;

                picture.SaveAs(path);

                picturesJSON.Add(new { pictureURL = string.Format("{0}{1}", folderPath.Replace("~", ""), fileName), pictureValue = string.Format("{0}{1}", folderPath.Replace("~/content/images/", ""), fileName) });
            }

            result.Data = picturesJSON;

            return result;
        }

        public JsonResult ChangeMode()
        {
            JsonResult result = new JsonResult();

            if(SessionHelper.DarkMode.Equals("false"))
            {
                SessionHelper.DarkMode = "true";
            }
            else SessionHelper.DarkMode = "false";

            result.Data = new { Success = true, DarkModeEnabled = SessionHelper.DarkMode };

            return result;
        }

        public ActionResult ExternalSocialScripts()
        {
            return PartialView("_ExternalSocialScripts");
        }
    }
}