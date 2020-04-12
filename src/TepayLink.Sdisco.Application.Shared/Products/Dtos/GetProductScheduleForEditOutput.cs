using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductScheduleForEditOutput
    {
		public CreateOrEditProductScheduleDto ProductSchedule { get; set; }

		public string ProductName { get; set;}


    }
}