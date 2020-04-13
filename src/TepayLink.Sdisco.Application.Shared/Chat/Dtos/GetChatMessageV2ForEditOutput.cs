using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetChatMessageV2ForEditOutput
    {
		public CreateOrEditChatMessageV2Dto ChatMessageV2 { get; set; }


    }
}