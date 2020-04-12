using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Chat
{
	[Table("Chatconversations")]
    public class Chatconversation : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long ChatConversationId { get; set; }
		
		public virtual long UserId { get; set; }
		
		public virtual string Message { get; set; }
		
		public virtual int Side { get; set; }
		
		public virtual int ReadState { get; set; }
		
		public virtual int ReceiverReadState { get; set; }
		
		public virtual string SharedMessageId { get; set; }
		

    }
}