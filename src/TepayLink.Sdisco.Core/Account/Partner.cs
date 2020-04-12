using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("Partners")]
    public class Partner : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual string WebsiteUrl { get; set; }
		
		public virtual string Languages { get; set; }
		
		public virtual string SkypeId { get; set; }
		
		public virtual string Comment { get; set; }
		
		public virtual string AffiliateKey { get; set; }
		
		public virtual byte Status { get; set; }
		
		public virtual bool AlreadyBecomeSdiscoPartner { get; set; }
		
		public virtual bool HasDriverLicense { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual long? DetinationId { get; set; }
		
        [ForeignKey("DetinationId")]
		public Detination DetinationFk { get; set; }
		
    }
}