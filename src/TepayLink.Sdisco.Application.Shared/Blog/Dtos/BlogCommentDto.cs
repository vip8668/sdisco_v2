
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class BlogCommentDto : EntityDto<long>
    {
		public string Email { get; set; }

		public string FullName { get; set; }

		public double Rating { get; set; }

		public string WebSite { get; set; }

		public string Title { get; set; }

		public string Comment { get; set; }


		 public long? BlogPostId { get; set; }

		 
    }
}