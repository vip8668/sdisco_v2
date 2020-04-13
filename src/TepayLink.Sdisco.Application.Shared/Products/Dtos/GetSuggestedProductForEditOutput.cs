using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetSuggestedProductForEditOutput
    {
		public CreateOrEditSuggestedProductDto SuggestedProduct { get; set; }

		public string ProductName { get; set;}

		public string ProductName2 { get; set;}


    }
}