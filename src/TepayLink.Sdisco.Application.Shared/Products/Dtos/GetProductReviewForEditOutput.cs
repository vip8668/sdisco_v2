using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetProductReviewForEditOutput
    {
		public CreateOrEditProductReviewDto ProductReview { get; set; }

		public string ProductName { get; set;}


    }
}