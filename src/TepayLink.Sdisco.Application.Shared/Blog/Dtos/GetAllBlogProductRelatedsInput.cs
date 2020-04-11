﻿using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetAllBlogProductRelatedsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string BlogPostTitleFilter { get; set; }

		 		 public string ProductNameFilter { get; set; }

		 
    }
}