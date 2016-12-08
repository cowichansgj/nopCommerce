using AutoMapper;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using Nop.Plugin.Commissions.ProductExtension.Models;
using Nop.Plugin.Commissions.ProductExtension.Services;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Commissions.ProductExtension.Controllers
{
    public class CommissionsProductExtensionController : BaseController
    {
        private readonly IProductCommissionService _productExtensionService;
        public CommissionsProductExtensionController(IProductCommissionService productExtensionService)
        {
            _productExtensionService = productExtensionService;
        }

        public ActionResult Edit(object additionalData)
        {
            if (additionalData == null)
                additionalData = 0;

            var productCommission = _productExtensionService.GetProductCommissionById((int)additionalData);

            if(productCommission == null && (int)additionalData != 0)
            {
                productCommission = new ProductCommission()
                {
                    Id = (int)additionalData
                };

                _productExtensionService.InsertProductCommission(productCommission);
            }

            return View(
                "~/Plugins/Commissions.ProductExtension/Views/CommissionsProductExtension/Edit.cshtml",
                Mapper.Map<ProductExtensionsModel>(productCommission));
        }

        [HttpPost]
        public ActionResult Update(ProductExtensionsModel model)
        {
            var productCommission = _productExtensionService.GetProductCommissionById(model.ProductId);
            Mapper.Map(model, productCommission);

            if (productCommission.Id != 0)
            {
                _productExtensionService.UpdateProductCommission(productCommission);
            }

            return Json(model);
        }
    }
}
