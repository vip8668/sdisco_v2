using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductDetailForEditOutput
    {
		public CreateOrEditProductDetailDto ProductDetail { get; set; }

		public string ProductName { get; set;}


    }
}