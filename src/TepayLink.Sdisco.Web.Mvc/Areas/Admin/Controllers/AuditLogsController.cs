using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Auditing;
using TepayLink.Sdisco.Auditing.Dto;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Web.Areas.Admin.Models.AuditLogs;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : SdiscoControllerBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> EntityChangeDetailModal(EntityChangeListDto entityChangeListDto)
        {
            var output = await _auditLogAppService.GetEntityPropertyChanges(entityChangeListDto.Id);

            var viewModel = new EntityChangeDetailModalViewModel(output, entityChangeListDto);

            return PartialView("_EntityChangeDetailModal", viewModel);
        }
    }
}