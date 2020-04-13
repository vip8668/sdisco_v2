using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class GetShareTransactionForEditOutput
    {
		public CreateOrEditShareTransactionDto ShareTransaction { get; set; }


    }
}