using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BlogProductRelateds;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Blog.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_BlogProductRelateds)]
    public class BlogProductRelatedsController : SdiscoControllerBase
    {
        private readonly IBlogProductRelatedsAppService _blogProductRelatedsAppService;

        public BlogProductRelatedsController(IBlogProductRelatedsAppService blogProductRelatedsAppService)
        {
            _blogProductRelatedsAppService = blogProductRelatedsAppService;
        }

        public ActionResult Index()
        {
            var model = new BlogProductRelatedsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_BlogProductRelateds_Create, AppPermissions.Pages_BlogProductRelateds_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBlogProductRelatedForEditOutput getBlogProductRelatedForEditOutput;

			if (id.HasValue){
				getBlogProductRelatedForEditOutput = await _blogProductRelatedsAppService.GetBlogProductRelatedForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBlogProductRelatedForEditOutput = new GetBlogProductRelatedForEditOutput{
					BlogProductRelated = new CreateOrEditBlogProductRelatedDto()
				};
			}

            var viewModel = new CreateOrEditBlogProductRelatedModalViewModel()
            {
				BlogProductRelated = getBlogProductRelatedForEditOutput.BlogProductRelated,
					BlogPostTitle = getBlogProductRelatedForEditOutput.BlogPostTitle,
					ProductName = getBlogProductRelatedForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBlogProductRelatedModal(long id)
        {
			var getBlogProductRelatedForViewDto = await _blogProductRelatedsAppService.GetBlogProductRelatedForView(id);

            var model = new BlogProductRelatedViewModel()
            {
                BlogProductRelated = getBlogProductRelatedForViewDto.BlogProductRelated
                , BlogPostTitle = getBlogProductRelatedForViewDto.BlogPostTitle 

                , ProductName = getBlogProductRelatedForViewDto.ProductName 

            };

            return PartialView("_ViewBlogProductRelatedModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BlogProductRelateds_Create, AppPermissions.Pages_BlogProductRelateds_Edit)]
        public PartialViewResult BlogPostLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BlogProductRelatedBlogPostLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BlogProductRelatedBlogPostLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_BlogProductRelateds_Create, AppPermissions.Pages_BlogProductRelateds_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BlogProductRelatedProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BlogProductRelatedProductLookupTableModal", viewModel);
        }

    }
}