using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetSaveItemForEditOutput
    {
		public CreateOrEditSaveItemDto SaveItem { get; set; }

		public string ProductName { get; set;}


    }
}