using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetAllBlogCommentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string EmailFilter { get; set; }

		public string FullNameFilter { get; set; }


		 public string BlogPostTitleFilter { get; set; }

		 
    }
}