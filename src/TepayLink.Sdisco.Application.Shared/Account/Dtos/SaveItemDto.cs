
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class SaveItemDto : EntityDto<long>
    {

		 public long? ProductId { get; set; }

		 
    }
}