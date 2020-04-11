using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductForEditOutput
    {
		public CreateOrEditProductDto Product { get; set; }

		public string CategoryName { get; set;}

		public string UserName { get; set;}

		public string PlaceName { get; set;}


    }
}