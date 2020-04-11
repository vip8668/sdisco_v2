using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetAllBlogPostsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}