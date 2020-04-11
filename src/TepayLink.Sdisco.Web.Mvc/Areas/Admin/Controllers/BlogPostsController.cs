using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BlogPosts;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Blog.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BlogPosts)]
    public class BlogPostsController : SdiscoControllerBase
    {
        private readonly IBlogPostsAppService _blogPostsAppService;

        public BlogPostsController(IBlogPostsAppService blogPostsAppService)
        {
            _blogPostsAppService = blogPostsAppService;
        }

        public ActionResult Index()
        {
            var model = new BlogPostsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BlogPosts_Create, AppPermissions.Pages_Administration_BlogPosts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBlogPostForEditOutput getBlogPostForEditOutput;

			if (id.HasValue){
				getBlogPostForEditOutput = await _blogPostsAppService.GetBlogPostForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBlogPostForEditOutput = new GetBlogPostForEditOutput{
					BlogPost = new CreateOrEditBlogPostDto()
				};
				getBlogPostForEditOutput.BlogPost.PublishDate = DateTime.Now;
			}

            var viewModel = new CreateOrEditBlogPostModalViewModel()
            {
				BlogPost = getBlogPostForEditOutput.BlogPost
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBlogPostModal(long id)
        {
			var getBlogPostForViewDto = await _blogPostsAppService.GetBlogPostForView(id);

            var model = new BlogPostViewModel()
            {
                BlogPost = getBlogPostForViewDto.BlogPost
            };

            return PartialView("_ViewBlogPostModal", model);
        }


    }
}