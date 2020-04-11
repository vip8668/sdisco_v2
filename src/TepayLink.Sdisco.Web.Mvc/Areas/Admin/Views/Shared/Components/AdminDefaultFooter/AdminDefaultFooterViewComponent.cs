using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Layout;
using TepayLink.Sdisco.Web.Session;
using TepayLink.Sdisco.Web.Views;

namespace TepayLink.Sdisco.Web.Areas.Admin.Views.Shared.Components.AdminDefaultFooter
{
    public class AdminDefaultFooterViewComponent : SdiscoViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminDefaultFooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
