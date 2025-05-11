using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Web.Areas.Dashboard.ViewModels;
using eCommerce.Shared.Helpers;
using eCommerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Shared.Enums;

namespace eCommerce.Web.Areas.Dashboard.Controllers
{
    public class PromosController : DashboardBaseController
    {
        public ActionResult Index(string searchTerm, int? pageNo)
        {
            var pageSize = (int)RecordSizeEnums.Size10;

            PromosListingViewModel model = new PromosListingViewModel
            {
                SearchTerm = searchTerm
            };

            model.Promos = PromosService.Instance.SearchPromos(searchTerm, pageNo, pageSize, out int count);

            model.Pager = new Pager(count, pageNo, pageSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            PromoActionViewModel model = new PromoActionViewModel();

            if (ID.HasValue)
            {
                var promo = PromosService.Instance.GetPromoByID(ID.Value);

                if (promo == null) return HttpNotFound();

                model.ID = promo.ID;
                model.PromoType = promo.PromoType;
                model.Name = promo.Name;
                model.Description = promo.Description;
                model.Code = promo.Code;
                model.Value = promo.Value;
                model.ValidTill = promo.ValidTill;
            }

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Action(PromoActionViewModel model)
        {
            JsonResult json = new JsonResult();

            try
            {
                if (model.ID > 0)
                {
                    var promo = PromosService.Instance.GetPromoByID(model.ID);

                    if (promo == null)
                    {
                        throw new Exception("Dashboard.Promos.Action.Validation.PromoNotFound".LocalizedString());
                    }

                    promo.ID = model.ID;
                    promo.PromoType = model.PromoType;
                    promo.Name = model.Name;
                    promo.Description = model.Description;
                    promo.Code = model.Code;
                    promo.Value = model.Value;
                    promo.ValidTill = model.ValidTill;

                    if (!PromosService.Instance.UpdatePromo(promo))
                    {
                        throw new Exception("Dashboard.Promos.Action.Validation.UnableToUpdatePromo".LocalizedString());
                    }

                    json.Data = new { Success = true };
                }
                else
                {
                    Promo promo = new Promo
                    {
                        ID = model.ID,
                        PromoType = model.PromoType,
                        Name = model.Name,
                        Description = model.Description,
                        Code = model.Code,
                        Value = model.Value,
                        ValidTill = model.ValidTill
                    };

                    if (!PromosService.Instance.SavePromo(promo))
                    {
                        throw new Exception("Dashboard.Promos.Action.Validation.UnableToCreatePromo".LocalizedString());
                    }

                    json.Data = new { Success = true };
                }
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = ex.Message };
            }

            return json;
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var operation = PromosService.Instance.DeletePromo(ID);

                result.Data = new { Success = operation, Message = operation ? string.Empty : "Dashboard.Promos.Action.Validation.UnableToDeletePromo".LocalizedString() };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = string.Format("{0}", ex.Message) };
            }

            return result;
        }
    }
}