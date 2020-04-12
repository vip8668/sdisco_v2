using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllPartnersForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string WebsiteUrlFilter { get; set; }

		public string LanguagesFilter { get; set; }

		public string SkypeIdFilter { get; set; }

		public string CommentFilter { get; set; }

		public string AffiliateKeyFilter { get; set; }

		public byte? MaxStatusFilter { get; set; }
		public byte? MinStatusFilter { get; set; }

		public int AlreadyBecomeSdiscoPartnerFilter { get; set; }

		public int HasDriverLicenseFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string DetinationNameFilter { get; set; }

		 
    }
}