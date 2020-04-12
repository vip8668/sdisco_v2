
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class CreateOrEditSaveItemDto : EntityDto<long?>
    {

		 public long? ProductId { get; set; }
		 
		 
    }
}