using eCommerce.Entities;
using eCommerce.Shared.Commons;
using eCommerce.Shared.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace eCommerce.Shared.Helpers
{
    public static class PictureHelper
    {
        public static MvcHtmlString Picture(this HtmlHelper htmlHelper, Picture picture, string classes = "", string style = "", string alt = "", string defaultPic = eCommerceConstants.DEFAULT_PICTURE, bool lazyLoad = true)
        {
            var picURL = picture != null ? picture.URL : "";

            return Picture(htmlHelper: htmlHelper, pictureURL: picURL, classes: classes, style: style, alt: alt, defaultPic: defaultPic, lazyLoad: lazyLoad);
        }
        public static MvcHtmlString PictureWithZoomAttribute(this HtmlHelper htmlHelper, Picture picture, string classes = "", string style = "", string alt = "", string attributeName = "data-zoom", string id = "")
        {
            var picURL = picture != null ? picture.URL : "";

            return PictureWithZoomAttribute(htmlHelper: htmlHelper, pictureURL: picURL, classes: classes, style: style, alt: alt, attributeName: attributeName, id: id);
        }

        public static MvcHtmlString Picture(this HtmlHelper htmlHelper, string pictureURL, string classes = "", string style = "", string alt = "", string defaultPic = eCommerceConstants.DEFAULT_PICTURE, bool lazyLoad = true)
        {
            pictureURL = string.IsNullOrEmpty(pictureURL) ? defaultPic : pictureURL;

            var image = new TagBuilder("img");
            image.AddCssClass(classes);
            image.MergeAttribute("style", style);

            if (lazyLoad)
            {
                image.MergeAttribute("data-src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, pictureURL));
                image.MergeAttribute("src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.LOADING_PICTURE));
            }
            else
            {
                image.MergeAttribute("src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, pictureURL));
            }

            image.MergeAttribute("alt", alt);
            image.MergeAttribute("onerror", string.Format("this.onerror=null;this.src='{0}{1}';", eCommerceConstants.MAIN_IMAGES_DIRECTORY, defaultPic));

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }
        public static MvcHtmlString PictureWithZoomAttribute(this HtmlHelper htmlHelper, string pictureURL, string classes = "", string style = "", string alt = "", string defaultPic = eCommerceConstants.DEFAULT_PICTURE, string attributeName = "data-zoom", string id = "")
        {
            pictureURL = string.IsNullOrEmpty(pictureURL) ? defaultPic : pictureURL;

            var image = new TagBuilder("img");
            image.AddCssClass(classes);
            image.MergeAttribute("style", style);
            image.MergeAttribute("src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, pictureURL));
            image.MergeAttribute(attributeName, string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, pictureURL));
            image.MergeAttribute("id", id);
            image.MergeAttribute("alt", alt);
            image.MergeAttribute("onerror", string.Format("this.onerror=null;this.src='{0}{1}';", eCommerceConstants.MAIN_IMAGES_DIRECTORY, defaultPic));

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString UserAvatar(this HtmlHelper htmlHelper, Picture picture, string classes = "", string style = "", string alt = "", bool lazyLoad = true)
        {
            var picURL = picture != null ? picture.URL : "";

            return Picture(htmlHelper: htmlHelper, pictureURL: picURL, classes: classes, style: style, alt: alt, defaultPic: eCommerceConstants.USER_DEFAULT_PICTURE, lazyLoad: lazyLoad);
        }

        public static MvcHtmlString UserAvatar(this HtmlHelper htmlHelper, string pictureURL, string classes = "", string style = "", string alt = "", bool lazyLoad = true)
        {
            return Picture(htmlHelper: htmlHelper, pictureURL: pictureURL, classes: classes, style: style, alt: alt, defaultPic: eCommerceConstants.USER_DEFAULT_PICTURE, lazyLoad: lazyLoad);
        }

        public static string PictureSource(this HtmlHelper htmlHelper, string pictureURL)
        {
            return string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, pictureURL);
        }
        public static string PictureSource(this HtmlHelper htmlHelper, Picture picture, string defaultPic = eCommerceConstants.DEFAULT_PICTURE)
        {
            if (picture != null)
            {
                return string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, picture.URL);
            }
            else return string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, defaultPic);
        }

        public static string PageImageURL(string imagePath, bool isCompletePath = false)
        {
            return string.Format("{0}{1}{2}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, isCompletePath ? string.Empty : "site/pages/", imagePath);
        }
        public static string UserAvatarSource(Picture picture)
        {
            var picURL = picture != null ? picture.URL : "";

            return string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, !string.IsNullOrEmpty(picURL) ? picURL : eCommerceConstants.USER_DEFAULT_PICTURE);
        }

        public static string LanguageIcon(this HtmlHelper htmlHelper, string iconCodeWithExtension)
        {
            return string.Format("{0}site/flags/{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, iconCodeWithExtension);
        }

        public static MvcHtmlString Thumbnail(this HtmlHelper htmlHelper, int pictureID, string pictureURL, int targetSize = 75, string seoTitle = "", string classes = "", string style = "", string alt = "", string defaultPic = eCommerceConstants.DEFAULT_PICTURE, bool lazyLoad = true)
        {
            var thumbnail = string.Empty;

            try
            {
                //fix for when file name becomes too long exception
                seoTitle = seoTitle.SafeSubstring(50);

                if (!string.IsNullOrEmpty(pictureURL))
                {
                    string thumbnailName = string.Format("{0}_{1}{2}{3}", pictureID, targetSize, (!string.IsNullOrEmpty(seoTitle) ? string.Format("_{0}_", seoTitle) : string.Empty), Path.GetExtension(pictureURL));

                    var thumbnailFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~"+ string.Format("{0}", eCommerceConstants.IMAGES_THUMBNAIL_DIRECTORY)) , thumbnailName);

                    using (var mutex = new Mutex(false, thumbnailName))
                    {
                        //if thumbnails directory is not already there, create it.
                        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(@"~" + string.Format("{0}", eCommerceConstants.IMAGES_THUMBNAIL_DIRECTORY))))
                        {
                            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(@"~" + string.Format("{0}", eCommerceConstants.IMAGES_THUMBNAIL_DIRECTORY)));
                        }

                        if (File.Exists(thumbnailFile))
                            thumbnail = thumbnailName;

                        mutex.WaitOne();

                        //check, if the file was created, while we were waiting for the release of the mutex.
                        if (!File.Exists(thumbnailFile))
                        {
                            var pictureFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~" + string.Format("{0}", eCommerceConstants.MAIN_IMAGES_DIRECTORY)), pictureURL);

                            byte[] pictureBinary = File.Exists(pictureFilePath) ? File.ReadAllBytes(pictureFilePath) : new byte[0];

                            if (pictureBinary != null && pictureBinary.Length > 0)
                            {
                                using (var thumbnailImage = Image.Load(pictureBinary, out var imageFormat))
                                {
                                    thumbnailImage.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                                    {
                                        Mode = ResizeMode.Max,
                                        Size = CalculateDimensions(thumbnailImage.Size(), targetSize)
                                    }));

                                    thumbnailImage.Save(thumbnailFile);
                                }

                                thumbnail = thumbnailName;
                            }
                        }

                        mutex.ReleaseMutex();
                    }
                }

                if (!string.IsNullOrEmpty(thumbnail))
                {
                    thumbnail = string.Format("{0}{1}", eCommerceConstants.THUMBNAIL_FOLDERNAME, thumbnail);
                }
                else
                {
                    thumbnail = defaultPic;
                }
            }
            catch
            {
                thumbnail = pictureURL;
            }

            var image = new TagBuilder("img");
            image.AddCssClass(classes);
            image.MergeAttribute("style", style);

            if (lazyLoad)
            {
                image.MergeAttribute("data-src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, thumbnail));
                image.MergeAttribute("src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, eCommerceConstants.LOADING_PICTURE));
            }
            else
            {
                image.MergeAttribute("src", string.Format("{0}{1}", eCommerceConstants.MAIN_IMAGES_DIRECTORY, thumbnail));
            }

            if (thumbnail == pictureURL)
            {
                image.MergeAttribute("data-original", "true");
            }

            image.MergeAttribute("alt", alt);
            image.MergeAttribute("onerror", string.Format("this.onerror=null;this.src='{0}{1}';", eCommerceConstants.MAIN_IMAGES_DIRECTORY, defaultPic));

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Thumbnail(this HtmlHelper htmlHelper, Picture picture, int targetSize = 75, string seoTitle = "", string classes = "", string style = "", string alt = "", string defaultPic = eCommerceConstants.DEFAULT_PICTURE, bool lazyLoad = true)
        {
            var picURL = picture != null ? picture.URL : "";
            var picID = picture != null ? picture.ID : 0;

            return Thumbnail(htmlHelper: htmlHelper, pictureID: picID, pictureURL: picURL, targetSize: targetSize, seoTitle: seoTitle,
                             classes: classes, style: style, alt: alt, defaultPic: defaultPic, lazyLoad: lazyLoad);
        }

        private static Size CalculateDimensions(Size originalSize, int targetSize, bool ensureSizePositive = true)
        {
            float width, height;

            if (originalSize.Height > originalSize.Width)
            {
                // portrait
                width = originalSize.Width * (targetSize / (float)originalSize.Height);
                height = targetSize;
            }
            else
            {
                // landscape or square
                width = targetSize;
                height = originalSize.Height * (targetSize / (float)originalSize.Width);
            }

            if (!ensureSizePositive)
                return new Size((int)Math.Round(width), (int)Math.Round(height));

            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        public static Picture GetProductThumbnail(List<ProductPicture> productPictures, int thumbnailPictureID)
        {
            var thumbnail = new Picture();

            if (productPictures != null && productPictures.Count > 0)
            {
                var selectedThumbnail = productPictures.FirstOrDefault(x => x.PictureID == thumbnailPictureID);

                thumbnail = selectedThumbnail != null ? selectedThumbnail.Picture : productPictures.FirstOrDefault().Picture;
            }

            return thumbnail;
        }
    }
}