
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class BlogProductRelatedDto : EntityDto<long>
    {

		 public long? BlogPostId { get; set; }

		 		 public long? ProductId { get; set; }

		 
    }
}