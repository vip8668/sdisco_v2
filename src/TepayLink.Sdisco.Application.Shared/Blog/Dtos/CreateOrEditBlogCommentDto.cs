
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class CreateOrEditBlogCommentDto : EntityDto<long?>
    {

		 public long? BlogPostId { get; set; }
		 
		 
    }
}