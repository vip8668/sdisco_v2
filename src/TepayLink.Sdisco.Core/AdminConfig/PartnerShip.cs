using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.AdminConfig
{
	[Table("PartnerShips")]
    public class PartnerShip : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Logo { get; set; }
		
		public virtual string Title { get; set; }
		
		public virtual string Link { get; set; }
		
		public virtual int Order { get; set; }
		

    }
}