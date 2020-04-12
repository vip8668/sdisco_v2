using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class GetAllCashoutMethodTypesForExcelInput
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public string NoteFilter { get; set; }



    }
}