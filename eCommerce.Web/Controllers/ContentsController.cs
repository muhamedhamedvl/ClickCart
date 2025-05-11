using eCommerce.Shared.Extensions;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerce.Web.Controllers
{
    public class ContentsController : PublicBaseController
    {
        public ActionResult AboutUs()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "About Us",
                PageDescription = String.Format("Know more about us and the great work we do here at {0}.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("AboutUs").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("about-us.jpg")
            };

            return View(model);
        }

        public ActionResult ContactUs()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "Contact Us",
                PageDescription = string.Format("Contact {0} Team.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("ContactUs").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("contact-us.jpg")
            };

            return View(model);
        }

        public ActionResult Blog()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "Blog",
                PageDescription = string.Format("Latest updates from {0}.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("Blog").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("blog.jpg")
            };

            return View(model);
        }

        public ActionResult PrivacyPolicy()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "Privacy Policy",
                PageDescription = string.Format("Read {0} Privacy Policy.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("PrivacyPolicy").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("privacy-policy.jpg")
            };

            return View(model);
        }

        public ActionResult RefundPolicy()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "Refund Policy",
                PageDescription = string.Format("Read {0} Refund Policy.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("RefundPolicy").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("refund-policy.jpg")
            };

            return View(model);
        }

        public ActionResult TermsConditions()
        {
            PageViewModel model = new PageViewModel
            {
                PageTitle = "Terms & Conditions",
                PageDescription = string.Format("Read {0} Terms & Conditions.", ConfigurationsHelper.ApplicationName),
                PageURL = Url.RouteUrl("TermsConditions").ToSiteURL(),
                PageImageURL = PictureHelper.PageImageURL("terms-conditions.jpg")
            };

            return View(model);
        }
    }
}