using TepayLink.Sdisco.Blog;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class BlogPostDto : EntityDto<long>
    {
		public string Title { get; set; }

		public string ShortDescription { get; set; }

		public string Content { get; set; }

		public DateTime PublishDate { get; set; }

		public string ThumbImage { get; set; }

		public BlogStatusEnum Status { get; set; }



    }
}