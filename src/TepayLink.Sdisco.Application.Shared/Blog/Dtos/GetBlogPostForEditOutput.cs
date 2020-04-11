using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetBlogPostForEditOutput
    {
		public CreateOrEditBlogPostDto BlogPost { get; set; }


    }
}