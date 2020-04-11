
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class CreateOrEditBlogProductRelatedDto : EntityDto<long?>
    {

		 public long? BlogPostId { get; set; }
		 
		 		 public long? ProductId { get; set; }
		 
		 
    }
}