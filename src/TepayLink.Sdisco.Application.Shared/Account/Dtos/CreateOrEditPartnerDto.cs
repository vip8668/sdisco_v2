
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class CreateOrEditPartnerDto : EntityDto<long?>
    {

		public string Name { get; set; }
		
		
		public string WebsiteUrl { get; set; }
		
		
		public string Languages { get; set; }
		
		
		public string SkypeId { get; set; }
		
		
		public string Comment { get; set; }
		
		
		public string AffiliateKey { get; set; }
		
		
		public byte Status { get; set; }
		
		
		public bool AlreadyBecomeSdiscoPartner { get; set; }
		
		
		public bool HasDriverLicense { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public long? DetinationId { get; set; }
		 
		 
    }
}