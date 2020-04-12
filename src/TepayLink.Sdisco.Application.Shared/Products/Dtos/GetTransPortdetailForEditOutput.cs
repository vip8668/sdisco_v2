using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetTransPortdetailForEditOutput
    {
		public CreateOrEditTransPortdetailDto TransPortdetail { get; set; }

		public string ProductName { get; set;}


    }
}