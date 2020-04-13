using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Affiliate.Dtos
{
    public class GetShortLinkForEditOutput
    {
		public CreateOrEditShortLinkDto ShortLink { get; set; }


    }
}