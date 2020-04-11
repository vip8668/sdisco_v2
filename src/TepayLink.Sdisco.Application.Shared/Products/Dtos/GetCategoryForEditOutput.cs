using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetCategoryForEditOutput
    {
		public CreateOrEditCategoryDto Category { get; set; }


    }
}