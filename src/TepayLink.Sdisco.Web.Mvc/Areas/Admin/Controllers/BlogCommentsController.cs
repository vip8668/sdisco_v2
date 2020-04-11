using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BlogComments;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Blog.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BlogComments)]
    public class BlogCommentsController : SdiscoControllerBase
    {
        private readonly IBlogCommentsAppService _blogCommentsAppService;

        public BlogCommentsController(IBlogCommentsAppService blogCommentsAppService)
        {
            _blogCommentsAppService = blogCommentsAppService;
        }

        public ActionResult Index()
        {
            var model = new BlogCommentsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BlogComments_Create, AppPermissions.Pages_Administration_BlogComments_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBlogCommentForEditOutput getBlogCommentForEditOutput;

			if (id.HasValue){
				getBlogCommentForEditOutput = await _blogCommentsAppService.GetBlogCommentForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBlogCommentForEditOutput = new GetBlogCommentForEditOutput{
					BlogComment = new CreateOrEditBlogCommentDto()
				};
			}

            var viewModel = new CreateOrEditBlogCommentModalViewModel()
            {
				BlogComment = getBlogCommentForEditOutput.BlogComment,
					BlogPostTitle = getBlogCommentForEditOutput.BlogPostTitle
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBlogCommentModal(long id)
        {
			var getBlogCommentForViewDto = await _blogCommentsAppService.GetBlogCommentForView(id);

            var model = new BlogCommentViewModel()
            {
                BlogComment = getBlogCommentForViewDto.BlogComment
                , BlogPostTitle = getBlogCommentForViewDto.BlogPostTitle 

            };

            return PartialView("_ViewBlogCommentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BlogComments_Create, AppPermissions.Pages_Administration_BlogComments_Edit)]
        public PartialViewResult BlogPostLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BlogCommentBlogPostLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BlogCommentBlogPostLookupTableModal", viewModel);
        }

    }
}