using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetRelatedProductForEditOutput
    {
		public CreateOrEditRelatedProductDto RelatedProduct { get; set; }

		public string ProductName { get; set;}

		public string ProductName2 { get; set;}


    }
}