using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetBlogCommentForEditOutput
    {
		public CreateOrEditBlogCommentDto BlogComment { get; set; }

		public string BlogPostTitle { get; set;}


    }
}