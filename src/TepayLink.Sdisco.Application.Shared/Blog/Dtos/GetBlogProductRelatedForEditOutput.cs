using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetBlogProductRelatedForEditOutput
    {
		public CreateOrEditBlogProductRelatedDto BlogProductRelated { get; set; }

		public string BlogPostTitle { get; set;}

		public string ProductName { get; set;}


    }
}