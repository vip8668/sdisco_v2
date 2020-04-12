using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.TransPortdetails;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_TransPortdetails)]
    public class TransPortdetailsController : SdiscoControllerBase
    {
        private readonly ITransPortdetailsAppService _transPortdetailsAppService;

        public TransPortdetailsController(ITransPortdetailsAppService transPortdetailsAppService)
        {
            _transPortdetailsAppService = transPortdetailsAppService;
        }

        public ActionResult Index()
        {
            var model = new TransPortdetailsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_TransPortdetails_Create, AppPermissions.Pages_TransPortdetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetTransPortdetailForEditOutput getTransPortdetailForEditOutput;

			if (id.HasValue){
				getTransPortdetailForEditOutput = await _transPortdetailsAppService.GetTransPortdetailForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getTransPortdetailForEditOutput = new GetTransPortdetailForEditOutput{
					TransPortdetail = new CreateOrEditTransPortdetailDto()
				};
			}

            var viewModel = new CreateOrEditTransPortdetailModalViewModel()
            {
				TransPortdetail = getTransPortdetailForEditOutput.TransPortdetail,
					ProductName = getTransPortdetailForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTransPortdetailModal(long id)
        {
			var getTransPortdetailForViewDto = await _transPortdetailsAppService.GetTransPortdetailForView(id);

            var model = new TransPortdetailViewModel()
            {
                TransPortdetail = getTransPortdetailForViewDto.TransPortdetail
                , ProductName = getTransPortdetailForViewDto.ProductName 

            };

            return PartialView("_ViewTransPortdetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransPortdetails_Create, AppPermissions.Pages_TransPortdetails_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new TransPortdetailProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TransPortdetailProductLookupTableModal", viewModel);
        }

    }
}