﻿using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllCategoriesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int ProductTypeFilter { get; set; }



    }
}