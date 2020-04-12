using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductUtilityForEditOutput
    {
		public CreateOrEditProductUtilityDto ProductUtility { get; set; }

		public string ProductName { get; set;}

		public string UtilityName { get; set;}


    }
}