using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductDetailComboForEditOutput
    {
		public CreateOrEditProductDetailComboDto ProductDetailCombo { get; set; }

		public string ProductName { get; set;}

		public string ProductDetailTitle { get; set;}

		public string ProductName2 { get; set;}


    }
}