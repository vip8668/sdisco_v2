
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class UserSubcriberDto : EntityDto<long>
    {
		public string Email { get; set; }



    }
}