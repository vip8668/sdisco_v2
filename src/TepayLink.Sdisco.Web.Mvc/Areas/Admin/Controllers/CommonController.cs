using System.Collections.Generic;
using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization.Permissions;
using TepayLink.Sdisco.Authorization.Permissions.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common.Modals;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize]
    public class CommonController : SdiscoControllerBase
    {
        private readonly IPermissionAppService _permissionAppService;

        public CommonController(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }

        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }

        public PartialViewResult EntityTypeHistoryModal(EntityHistoryModalViewModel input)
        {
            return PartialView("Modals/_EntityTypeHistoryModal", ObjectMapper.Map<EntityHistoryModalViewModel>(input));
        }

        public PartialViewResult PermissionTreeModal(List<string> grantedPermissionNames = null)
        {
            var permissions = _permissionAppService.GetAllPermissions().Items.ToList();

            var model = new PermissionTreeModalViewModel
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissionNames
            };

            return PartialView("Modals/_PermissionTreeModal", model);
        }

        public PartialViewResult InactivityControllerNotifyModal()
        {
            return PartialView("Modals/_InactivityControllerNotifyModal");
        }
    }
}